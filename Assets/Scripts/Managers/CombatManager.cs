using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CombatManager : IManager
{
    public Action<int> currentCardAction;


    public override void PostAwake()
    {

    }

    public override void PreUpdate()
    {

    }

    public override void PostUpdate()
    {
    }
    public void WaitingForDice()
    {
        GameManager.Instance.diceSystem.ReceiveAction(currentCardAction);
    }


}
