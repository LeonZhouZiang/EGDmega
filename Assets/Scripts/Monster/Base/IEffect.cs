using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public interface IEffect
{
    public abstract Task ActionEffect();
    public abstract Task GetTargets(Vector3[] range);
}

/// <summary>
/// 对范围1的一个目标造成1伤害
/// </summary>
public abstract class MonsterAttackEffect : MonoBehaviour, IEffect
{
    public string description;
    [HideInInspector]
    public List<Survivor> targets;

    public int damage = 1;
    public int searchRange = 1;
    public List<Survivor> Targets => targets;

    public virtual async Task GetTargets(Vector3[] range)
    {
        await GameManager.Instance.combatManager.monster.GetComponent<Monster>().ShowStateText("Finding target");
        Vector3[] myRange = new Vector3[range.Length];
        //Get nearest player by default
        Vector3 target = Vector3.zero;
        if (GameManager.Instance.combatManager.GetNearestPlayerPos(searchRange, out target))
        {
            //apply aoe area
            for(int i = 0; i < range.Length; i++)
            {
                myRange[i] = target + range[i] * GameManager.Instance.mapManager.Scale;
            }
        
            targets = GameManager.Instance.combatManager.GetSurvivorsInRange(myRange);
        }
    }

    public virtual async Task ActionEffect()
    {
        await GameManager.Instance.combatManager.monster.GetComponent<Monster>().ShowStateText("Taking action!");
        Debug.Log("Effect start");
        if (targets.Count != 0 )
            GameManager.Instance.combatManager.SetupHitQueue(targets, damage);
        else
        {
            await GameManager.Instance.combatManager.monster.GetComponent<Monster>().ShowStateText("No target found.");
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
    public string description;
    public int moveStep = -1;
    private List<Vector3> pathway = new();

    /// <summary>
    /// Get nearest player
    /// </summary>
    /// <param name="range"></param>
    /// <returns></returns>
    public async Task GetTargets(Vector3[] range)
    {
        await GameManager.Instance.combatManager.monster.GetComponent<Monster>().ShowStateText("Finding target.");
        Monster monster = GameManager.Instance.combatManager.monster.GetComponent<Monster>();
        if (moveStep == -1) Debug.LogWarning("Not setup");

        Vector3 target; 
        GameManager.Instance.combatManager.GetNearestPlayerPos(99,out target);
        Node[] path = GameManager.Instance.astar.TryFindPath(monster.transform.position, target, moveStep);
        foreach(var node in path)
        {
            pathway.Add(node.WorldPosition);
        }
    }

    /// <summary>
    /// Basic move
    /// </summary>
    public virtual async Task ActionEffect()
    {
        await GameManager.Instance.combatManager.monster.GetComponent<Monster>().ShowStateText("Taking action!");
        if (pathway.Count != 0)
        {
            await GameManager.Instance.combatManager.monster.GetComponent<Unit>().MovePath(pathway);
            
            if (GameManager.Instance.combatManager.isPreActionPhase)
                GameManager.Instance.combatManager.DoActionInQueueRecursively();
            else
                GameManager.Instance.combatManager.DoMonsterInstanceActionsRecursively();
            
        }
        else
        {
            await GameManager.Instance.combatManager.monster.GetComponent<Monster>().ShowStateText("No target found.");
            Debug.Log("no available target, next act");
            if (GameManager.Instance.combatManager.isPreActionPhase)
                GameManager.Instance.combatManager.DoActionInQueueRecursively();
            else
                GameManager.Instance.combatManager.DoMonsterInstanceActionsRecursively();
        }
    }
}