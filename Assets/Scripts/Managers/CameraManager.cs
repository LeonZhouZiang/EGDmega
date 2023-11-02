using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CameraManager : MonoBehaviour
{
    public static CameraManager Instance;

    protected internal GameObject mainCamera;
    protected internal Vector3 startPos;
    protected internal Quaternion startRotation;
    [Range(0.1f,1)]
    public float speed;
    public Vector3 positionOffset;
    public Quaternion rotationOffset;
    public void Awake()
    {
        Instance = this;
        mainCamera = Camera.main.gameObject;
        startPos = mainCamera.transform.position;
        startRotation = mainCamera.transform.rotation;
    }

    public void MoveToTarget(Vector3 target)
    {
        StopAllCoroutines();
        StartCoroutine(MoveTo(target));
    }

    public void ResetPosition()
    {
        StopAllCoroutines();
        StartCoroutine(ResetPos());
    }

    private IEnumerator MoveTo(Vector3 target)
    {
        while(Vector3.SqrMagnitude(mainCamera.transform.position - positionOffset) > 0.01f)
        {
            mainCamera.transform.position = Vector3.Lerp(mainCamera.transform.position, positionOffset + target, speed);
            mainCamera.transform.rotation = Quaternion.Lerp(mainCamera.transform.rotation, rotationOffset, speed);
            yield return null;
        }
    }

    private IEnumerator ResetPos()
    {
        while (Vector3.SqrMagnitude(mainCamera.transform.position - startPos) > 0.01f)
        {
            mainCamera.transform.position = Vector3.Lerp(mainCamera.transform.position, startPos, speed);
            mainCamera.transform.rotation = Quaternion.Lerp(mainCamera.transform.rotation, startRotation, speed);
            yield return null;
        }
    }
}
