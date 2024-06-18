using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterActionCard : MonoBehaviour
{
    [HideInInspector]
    public string cardName;
    [HideInInspector]
    public List<Phase> phaseList = new();
    [HideInInspector]
    public Sprite image;
    [HideInInspector]
    public SingleAction lastAction;
    [HideInInspector]
    public ActionBar actionBar;

    [SerializeField]
    public MonsterActionCardInfo cardInfo;

    public void Initialize()
    {
        cardName = cardInfo.cardName;
        phaseList = cardInfo.phaseList;
        image = cardInfo.image;

        actionBar = new(phaseList);

        lastAction = phaseList[^1].actions[^1];

        //set owners
        foreach(var phase in phaseList)
        {
            for(int i = 0; i < phase.actions.Count; i++)
            {
                phase.actions[i].Owner = GameManager.Instance.combatManager.Monster;
            }
        }
    }
}


[System.Serializable]
public class Phase
{
    public int prepareTime;
    public List<SingleAction> actions;
}


public class ActionBar
{
    public static Sprite imageAttack;
    public static Sprite imageMove;
    public List<SingleAction> actions;

    public ActionBar(List<Phase> phase)
    {

    }
}


[System.Serializable]
public class MonsterActionCardInfo
{
    public string cardName;
    public List<Phase> phaseList = new();
    public Sprite image;
    public ActionBar actionBar;
}