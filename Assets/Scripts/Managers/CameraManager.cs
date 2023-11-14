using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

[System.Serializable]
public class CameraManager : MonoBehaviour
{
    public static CameraManager Instance;

    protected internal GameObject mainCamera;
    protected internal Vector3 startPos;
    protected internal Quaternion startRotation;

    public float speed;
    public Vector3 positionOffset;
    public Quaternion rotationOffset;

    TaskCompletionSource<bool> coroutineTask;
    public void Initialize()
    {
        Instance = this;
        mainCamera = Camera.main.gameObject;
        startPos = mainCamera.transform.position;
        startRotation = mainCamera.transform.rotation;
    }

    public async Task MoveToTarget(Vector3 target)
    {
        coroutineTask = new TaskCompletionSource<bool>();
        StopAllCoroutines();
        StartCoroutine(MoveTo(target));
        await coroutineTask.Task;
    }

    public async Task CenterToTarget(Vector3 target)
    {
        coroutineTask = new TaskCompletionSource<bool>();
        StopAllCoroutines();
        StartCoroutine(CenterTo(target));
        await coroutineTask.Task;
    }

    public async Task ResetPosition()
    {
        coroutineTask = new TaskCompletionSource<bool>();
        StopAllCoroutines();
        StartCoroutine(ResetPos());
        await coroutineTask.Task;
    }

    private IEnumerator MoveTo(Vector3 target)
    {
        while(Vector3.SqrMagnitude(mainCamera.transform.position - positionOffset - target) > 0.001f)
        {
            Vector3 newPosition = Vector3.Lerp(mainCamera.transform.position, target + positionOffset, Time.deltaTime * speed);
            Quaternion newRotation = Quaternion.Lerp(mainCamera.transform.rotation, rotationOffset, Time.deltaTime * speed);
            mainCamera.transform.position = newPosition;
            mainCamera.transform.rotation = newRotation;
            yield return null;
        }
        mainCamera.transform.position = positionOffset + target;
        coroutineTask.SetResult(true);
    }

    private IEnumerator ResetPos()
    {
        while (Vector3.SqrMagnitude(mainCamera.transform.position - startPos) > 0.001f)
        {
            mainCamera.transform.position = Vector3.Lerp(mainCamera.transform.position, startPos, Time.deltaTime * speed);
            mainCamera.transform.rotation = Quaternion.Lerp(mainCamera.transform.rotation, startRotation, Time.deltaTime * speed);
            yield return null;
        }
        mainCamera.transform.position = startPos;
        coroutineTask.SetResult(true);
    }

    private IEnumerator CenterTo(Vector3 target)
    {
        Vector3 targetPos = new Vector3(target.x, 0, target.z) + startPos;
        while (Vector3.SqrMagnitude(mainCamera.transform.position - targetPos) > 0.001f)
        {
            Vector3 newPosition = Vector3.Lerp(mainCamera.transform.position, target + positionOffset, Time.deltaTime * speed);
            Quaternion newRotation = Quaternion.Lerp(mainCamera.transform.rotation, rotationOffset, Time.deltaTime * speed);
            mainCamera.transform.position = newPosition;
            mainCamera.transform.rotation = newRotation;
            yield return null;
        }
        mainCamera.transform.position = targetPos;
        coroutineTask.SetResult(true);
    }

}
