using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Survivor : Unit
{
    public int totalHealth;
    public Dictionary<string, HumanBodyPart> bodyParts;
    public SingleAction basicMove;
    public SingleAction basicAttack;
    public SurvivorInfo survivorInfo;

    //only 1 waepon at this time
    public Item items;
    public void Gete()
    {
        
    }
}

[System.Serializable]
public class HumanBodyPart
{
    public string partName = "";
    public int health = 1;
    public int multiplier;
}

[System.Serializable]
public class SurvivorInfo
{
    public int totalHealth;
    public string name;

}
