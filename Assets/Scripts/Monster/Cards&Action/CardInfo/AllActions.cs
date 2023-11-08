using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AllActions : MonoBehaviour
{
    public List<IEffect> EffectList;
}


public interface IEffect
{
    public abstract void ActionEffect();
    public abstract bool GetTargets();

}

public abstract class MonsterAttackEffect : IEffect
{
    [HideInInspector]
    public List<Survivor> targets;
    public int damage = 1;

    public List<Survivor> Targets => targets;

    public virtual bool GetTargets()
    {
        //Get nearest player by default
        Vector3[] range = { GameManager.Instance.combatManager.GetNearestPlayerPos()};
        targets = GameManager.Instance.combatManager.GetSurvivorsInRange(range);
        return targets.Count == 0 ? false : true;
    }

    public virtual void ActionEffect()
    {
        GameManager.Instance.combatManager.SetupHitQueue(targets, damage);
    }

}

public abstract class MonsterMoveEffect : IEffect
{
    public int range = -1;
    private List<Vector3> pathway;

    public bool GetTargets()
    {
        Monster monster = GameManager.Instance.combatManager.monster.GetComponent<Monster>();
        if (range == -1) Debug.LogWarning("Not setup");

        Node[] path = GameManager.Instance.astar.TryFindPath(monster.transform.position, GameManager.Instance.combatManager.GetNearestPlayerPos(), range);
        foreach(var node in path)
        {
            pathway.Add(node.worldPosition);
        }
        return path.Length != 0;
    }

    public virtual void ActionEffect()
    {
        here
    }
}