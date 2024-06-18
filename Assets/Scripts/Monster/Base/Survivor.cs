using System.Collections.Generic;
using UnityEngine;

public class Survivor : Unit
{
    private int totalHealth;
    [HideInInspector]
    public string survivorName;

    public Dictionary<string, HumanBodyPart> bodyParts;
    

    public SurvivorInfo survivorInfo;

    //only 1 waepon at this time
    public Item weapon;

    [HideInInspector]
    public int moveDistance;
    [HideInInspector]
    public bool canMove;
    [HideInInspector]
    public bool canAttack;
    [HideInInspector]
    public int str;
    [HideInInspector]
    public int dex;

    [HideInInspector]
    public string aimmingPartition;
    [HideInInspector]
    public int waitingDamage;
    public override void Start()
    {
        base.Start();
        survivorName = survivorInfo.survivorName;
        unitName = survivorName;
        moveDistance = survivorInfo.moveDistance;
        str = survivorInfo.str;
        dex = survivorInfo.dex;

        bodyParts = new();
        bodyParts.Add("Head", new HumanBodyPart("Head", 3));
        bodyParts.Add("Body", new HumanBodyPart("Body", 3));
        bodyParts.Add("Arms", new HumanBodyPart("Arms", 3));
        bodyParts.Add("Legs", new HumanBodyPart("Legs", 3));

    }

    public void TakeDamage(string partName, int value)
    {
        if (!bodyParts.ContainsKey(partName)) Debug.LogError("invalid name");

        bodyParts[partName].health -= value;
        if(bodyParts[partName].health < 0)
        {
            bodyParts[partName].health = 0;
            DeathCheck();
        }
    }


    public void PlayerInjure(int value)
    {
        string part = "";

        if (value == 1)
        {
            part = "Head";
            TakeDamage("Head", waitingDamage);
        }
        if (value == 2)
        {
            part = "Body";
            TakeDamage("Body", waitingDamage);
        }
        if (value == 3 || value == 4)
        {
            part = "Arms";
            TakeDamage("Arms", waitingDamage);
        }
        if (value == 5 || value == 6)
        {
            part = "Legs";
            TakeDamage("Legs", waitingDamage);
        }

        GameManager.Instance.coroutineHelper.ShowHintText($"Took a hit to the {part}.");
        GameManager.Instance.diceSystem.HideExplanation();

        GameManager.Instance.combatManager.ProcessTargetsOneByOne();
    }


    public void RequireAttack()
    {
        if(canAttack && GameManager.Instance.mouseStateManager.allowedToClick)
            GameManager.Instance.mouseStateManager.RequireAttack(this, weapon.attackRange, SelectBodyPart);
    }
    public void RequireMove()
    {
        if(canMove && GameManager.Instance.mouseStateManager.allowedToClick)
            GameManager.Instance.mouseStateManager.RequireMove(this, moveDistance, Move);
    }

    public void SelectBodyPart(Monster monster)
    {
        GameManager.Instance.uiManager.ShowMonsterBodyParts(this);
    }

    public void Move(List<Vector3> path)
    {
        canMove = false;
        GameManager.Instance.uiManager.playerMoveBtn.interactable = false;
        MovePath(path);
    }

    public void PlayerAccuracyCheck(int diceValue)
    {
        canAttack = false;
        // critical
        if (diceValue == 6)
        {
            GameManager.Instance.coroutineHelper.ResultSuccess(true);
            GameManager.Instance.diceSystem.RequireAction(StrengthCheck, "Stength check");
            GameManager.Instance.combatManager.monster.GetComponent<Monster>().React(diceValue, aimmingPartition);
        }
        else
        {
            if (diceValue + dex >= weapon.dexRequirement && diceValue != 1)
            {
                GameManager.Instance.coroutineHelper.ResultSuccess(true);
                GameManager.Instance.diceSystem.RequireAction(StrengthCheck, "Stength check");
                GameManager.Instance.combatManager.monster.GetComponent<Monster>().React(diceValue, aimmingPartition);
            }
            //missed
            else
            {
                GameManager.Instance.coroutineHelper.ResultSuccess(false);
                GameManager.Instance.combatManager.monster.GetComponent<Monster>().React(diceValue, aimmingPartition);
                Debug.Log("Too small");
            }
        }
    }

    public void StrengthCheck(int diceValue)
    {
        if (diceValue == 6)
        {
            GameManager.Instance.coroutineHelper.ResultSuccess(true);
            GameManager.Instance.combatManager.HurtMonster(aimmingPartition, weapon.attackDamage);
        }
        else
        {
            if (diceValue + str + weapon.strOffset >= GameManager.Instance.combatManager.Monster.toughness && diceValue != 1)
            {
                GameManager.Instance.coroutineHelper.ResultSuccess(true);
                GameManager.Instance.combatManager.HurtMonster(aimmingPartition,weapon.attackDamage);
            }
            //too weak
            else
            {
                GameManager.Instance.coroutineHelper.ResultSuccess(false);
                Debug.Log("failed");
            }
        }
    }

    public void DeathCheck()
    {
        
    }
}

[System.Serializable]
public class HumanBodyPart
{
    public string partName = "";
    public int health = 3;
    public float multiplier;

    public HumanBodyPart(string name, int hp, float mul = 1)
    {
        partName = name;
        health = hp;
        multiplier = mul;
    }
}

[System.Serializable]
public class SurvivorInfo
{
    public string survivorName;
    public int str;
    public int dex;
    public int moveDistance;
}
