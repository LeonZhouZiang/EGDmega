using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SingleAction
{
    public string actionName;
    public enum ActionType { MOVE, ATTACK}
    public enum TargetType {GRID, UNIT }

    public ActionType actionType;
    public TargetType selectionType;

    //aoe range
    public Vector3[] Range = { Vector3.zero };

    public Effect effect;
}


