using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CombatManager : IManager
{
    public GameObject player1;
    public GameObject player2;
    public GameObject monster;

    public MonsterActionCard currentActionCard;
    public Action<int> currentCardAction;

    public Queue<GameObject> allUnits = new();
    public GameObject turnOwner;

    public override void PostAwake()
    {
        allUnits.Enqueue(player1);
        allUnits.Enqueue(player2);
        allUnits.Enqueue(monster);
    }

    public override void PreUpdate()
    {

    }

    
    public void GoNextTurn()
    {
        var tmp = allUnits.Dequeue();
        allUnits.Enqueue(tmp);

        turnOwner = allUnits.Peek();
        StartNewTurn();
    }

    public void StartNewTurn()
    {
        //check setup time 0 cards
        ActivatePreparedAction();
        //unit action
        if (turnOwner.CompareTag("Monster"))
        {
            monster.GetComponent<Monster>().CheckCurrentActionCard();
        }
        else if(turnOwner.CompareTag("Player"))
        {

        }
    }

    private void ActivatePreparedAction()
    {
        
        throw new NotImplementedException();
    }

    public void WaitingForDice()
    {
        GameManager.Instance.diceSystem.ReceiveAction(currentCardAction);
    }

    internal void Activate(MonsterActionCard actionCard)
    {
        currentActionCard = actionCard;

        throw new NotImplementedException();
    }
}
