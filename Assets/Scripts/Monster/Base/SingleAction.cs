using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SingleAction
{
    public string name;
    public enum TargetSelectType {GRID, UNIT }

    //Grid offset
    public Vector3[] Range = { Vector3.zero };


    public virtual void Effect(Unit targets)
    {
        
    }

   
}
