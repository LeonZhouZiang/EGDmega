using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class AOE : MonsterAttackEffect
{
    public override async Task GetTargets(Vector3[] range)
    {
        await GameManager.Instance.combatManager.monster.GetComponent<Monster>().ShowStateText("Finding target");
        Vector3[] myRange = new Vector3[range.Length];
        //Get nearest player by default
        Vector3 target = Vector3.zero;
        if (GameManager.Instance.combatManager.GetNearestPlayerPos(searchRange, out target))
        {
            //apply aoe area
            for (int i = 0; i < range.Length; i++)
            {
                myRange[i] = target + range[i] * GameManager.Instance.mapManager.Scale;
            }

            targets = GameManager.Instance.combatManager.GetSurvivorsInRange(myRange);
        }
    }

    public override async Task ActionEffect()
    {
        await GameManager.Instance.combatManager.monster.GetComponent<Monster>().ShowStateText("Taking action!");
        if (targets.Count != 0)
            GameManager.Instance.combatManager.SetupHitQueue(targets, damage);
        else
        {

            if (GameManager.Instance.combatManager.isPreActionPhase)
            {
                await GameManager.Instance.combatManager.monster.GetComponent<Monster>().ShowStateText("No target found.");
                GameManager.Instance.combatManager.DoActionInQueueRecursively();
            }
            else
            {
                await GameManager.Instance.combatManager.monster.GetComponent<Monster>().ShowStateText("No target found.");
                GameManager.Instance.combatManager.DoMonsterInstanceActionsRecursively();
            }
        }
    }
}
