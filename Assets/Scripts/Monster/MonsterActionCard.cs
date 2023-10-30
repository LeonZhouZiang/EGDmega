using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterActionCard : MonoBehaviour
{
    public string cardName;
    public SortedList<int, Phase> actionList = new();
    public Actions currentActionCard;

    public MonsterInfo info;
}

[System.Serializable]
public class Phase
{
    public int prepareTime;
    List<Actions> actions;
}
