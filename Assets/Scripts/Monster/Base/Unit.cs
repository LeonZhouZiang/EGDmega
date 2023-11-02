using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
    [SerializeField]
    public string unitName;
    [HideInInspector]
    public Vector3 worldPostition;

    [HideInInspector]
    public Vector3 orientation = Vector3.back;

    public void SetOrientation(Vector3 dir)
    {
        if(dir == Vector3.back || dir == Vector3.forward || dir == Vector3.left || dir == Vector3.right)
        {
            orientation = dir;
        }
        else
        {
            Debug.LogError("Invalid dir");
        }
    }
}
