using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AOE : MonsterAttackEffect
{
    public override bool GetTargets(Vector3[] range)
    {
        Vector3[] myRange = new Vector3[range.Length];
        //Get nearest player by default
        Vector3 target = Vector3.zero;
        if (GameManager.Instance.combatManager.GetNearestPlayerPos(searchRange, out target))
        {
            //apply aoe area
            for (int i = 0; i < range.Length; i++)
            {
                myRange[i] = target + range[i];
            }

            targets = GameManager.Instance.combatManager.GetSurvivorsInRange(myRange);
            return true;
        }
        //outta range
        else
        {
            return false;
        }
    }

    public override void ActionEffect()
    {
        if (targets.Count != 0)
            GameManager.Instance.combatManager.SetupHitQueue(targets, damage);
        else
            if (GameManager.Instance.combatManager.isPreActionPhase)
            GameManager.Instance.combatManager.DoActionInQueueRecursively();
        else
            GameManager.Instance.combatManager.DoMonsterInstanceActionsRecursively();
    }
}
