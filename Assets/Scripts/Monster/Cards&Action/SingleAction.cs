using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SingleAction
{
    public Unit owner;
    public string actionName;
    public enum ActionType { MOVE, ATTACK}
    public enum AttackType {GRID, DIRECTION}

    public ActionType actionType;
    public AttackType selectionType;

    //aoe(direction) range
    public Vector3[] Range = { Vector3.zero };

    public MonoBehaviour effect;
    public Unit Owner { get => owner; set => owner = value; }

    public bool GetTargets()
    {
        return (effect as IEffect).GetTargets(Range);
    }
}


