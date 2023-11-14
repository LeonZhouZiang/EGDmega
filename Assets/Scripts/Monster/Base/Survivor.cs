using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
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
    public int attackRange;
    [HideInInspector]
    public bool canMove;
    [HideInInspector]
    public bool canAttack;
    [HideInInspector]
    public int str;
    [HideInInspector]
    public int dex;

    public string aimmingPartition; 
    public override void Start()
    {
        base.Start();
        survivorName = survivorInfo.name;
        moveDistance = survivorInfo.moveDistance;
        str = survivorInfo.str;
        dex = survivorInfo.dex;
        attackRange = weapon.attackRange;

        bodyParts = new();
        bodyParts.Add("Head", new HumanBodyPart("Head", 2));
        bodyParts.Add("Body", new HumanBodyPart("Body", 2));
        bodyParts.Add("Arms", new HumanBodyPart("Arms", 2));
        bodyParts.Add("Legs", new HumanBodyPart("Legs", 2));

    }

    public void TakeDamage(string partName, int value)
    {
        bodyParts[partName].health -= value;
        if(bodyParts[partName].health < 0)
        {
            bodyParts[partName].health = 0;
            DeathCheck();
        }
    }

    public int waitingDamage;
    public void ChooseInjurePart(int value)
    {
        if(value == 1) TakeDamage("Head", waitingDamage);
        if (value == 2) TakeDamage("Body", waitingDamage);
        if (value == 3 || value == 4) TakeDamage("Hands", waitingDamage);
        if (value == 5 || value == 6) TakeDamage("Legs", waitingDamage);
        GameManager.Instance.combatManager.DoMonsterInstanceActionsRecursively();
    }


    public void RequireAttack()
    {
        if(canAttack)
            GameManager.Instance.mouseStateManager.RequireAttack(this, attackRange, SelectBodyPart);
    }
    public void RequireMove()
    {
        if(canMove)
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

    public void MonsterEvadeCheck(int diceValue)
    {
        canAttack = false;
        if (diceValue == 6)
        {
            GameManager.Instance.combatManager.monster.GetComponent<Monster>().React(diceValue, aimmingPartition);
            GameManager.Instance.diceSystem.RequireAction(StrengthCheck);
            GameManager.Instance.uiManager.UpdateStateText("Strength Check");
        }
        else
        {
            if (diceValue + dex >= weapon.dexRequirement)
            {
                GameManager.Instance.combatManager.monster.GetComponent<Monster>().React(diceValue, aimmingPartition);
            }
            else
                Debug.Log("Too small");
        }
    }

    public void StrengthCheck(int diceValue)
    {
        if (diceValue == 6)
        {

        }
        else
        {
            if (diceValue + str <= weapon.strRequirement)
            {

            }
            else
                Debug.Log("too small");
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
    public string name;
    public int str;
    public int dex;
    public int moveDistance;
}
