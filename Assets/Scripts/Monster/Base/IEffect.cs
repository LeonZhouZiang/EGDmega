using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IEffect
{
    public abstract void ActionEffect();
    public abstract bool GetTargets(Vector3[] range);
}

/// <summary>
/// 对范围1的一个目标造成1伤害
/// </summary>
public abstract class MonsterAttackEffect : MonoBehaviour, IEffect
{
    [HideInInspector]
    public List<Survivor> targets;

    public int damage = 1;
    public int searchRange = 1;
    public List<Survivor> Targets => targets;

    public virtual bool GetTargets(Vector3[] range)
    {
        Vector3[] myRange = new Vector3[range.Length];
        //Get nearest player by default
        Vector3 target = Vector3.zero;
        if (GameManager.Instance.combatManager.GetNearestPlayerPos(searchRange, out target))
        {
            //apply aoe area
            for(int i = 0; i < range.Length; i++)
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

    public virtual void ActionEffect()
    {
        Debug.Log("Effect start");
        if (targets.Count != 0 )
            GameManager.Instance.combatManager.SetupHitQueue(targets, damage);
        else
        {
            Debug.Log("no available target, next act");
            if (GameManager.Instance.combatManager.isPreActionPhase)
                GameManager.Instance.combatManager.DoActionInQueueRecursively();
            else
                GameManager.Instance.combatManager.DoMonsterInstanceActionsRecursively();
        }
    }

}


public abstract class MonsterMoveEffect : MonoBehaviour, IEffect
{
    /// <summary>
    /// should be changed
    /// </summary>
    public int moveStep = -1;
    private List<Vector3> pathway;

    /// <summary>
    /// Get nearest player
    /// </summary>
    /// <param name="range"></param>
    /// <returns></returns>
    public bool GetTargets(Vector3[] range)
    {
        Monster monster = GameManager.Instance.combatManager.monster.GetComponent<Monster>();
        if (moveStep == -1) Debug.LogWarning("Not setup");

        Vector3 target; 
        GameManager.Instance.combatManager.GetNearestPlayerPos(99,out target);
        Node[] path = GameManager.Instance.astar.TryFindPath(monster.transform.position, target, moveStep);
        foreach(var node in path)
        {
            pathway.Add(node.worldPosition);
        }
        return path.Length != 0;
    }

    /// <summary>
    /// Basic move
    /// </summary>
    public virtual void ActionEffect()
    {
        Debug.Log("Effect start");
        if (pathway.Count != 0)
        {
            GameManager.Instance.combatManager.monster.GetComponent<Unit>().MovePath(pathway);
        }
        else
        {
            Debug.Log("no available target, next act");
            if (GameManager.Instance.combatManager.isPreActionPhase)
                GameManager.Instance.combatManager.DoActionInQueueRecursively();
            else
                GameManager.Instance.combatManager.DoMonsterInstanceActionsRecursively();
        }
    }
}