using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Survivor : Unit
{
    private int totalHealth;
    [HideInInspector]
    public string survivorName;

    public Dictionary<string, HumanBodyPart> bodyParts;
    public SingleAction basicMove;
    public SingleAction basicAttack;
    public SurvivorInfo survivorInfo;

    //only 1 waepon at this time
    public Item weapon;

    public int str;
    public int dex;

    public void Start()
    {
        survivorName = survivorInfo.name;
        str = survivorInfo.str;
        dex = survivorInfo.dex;
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
}
