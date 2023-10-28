using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CombatManager : IManager
{
    public Action<int> currentCardAction;


    public void PostAwake()
    {

    }

    public void PreUpdate()
    {

    }

    public void PostUpdate()
    {
    }

    public void PreLateUpdate()
    {
 
    }
    public void PostLateUpdate()
    {

    }
    
    public void WaitingForDice()
    {
        GameManager.Instance.diceSystem.ReceiveAction(currentCardAction);
    }


}
