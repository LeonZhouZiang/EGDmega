using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class MonsterActionCard
{
    public string cardName;
    public SortedList<int, Phase> actionList = new();
    public GameObject currentAction;
    public Sprite image;
    public ActionBar actionBar;

}

[System.Serializable]
public class Phase
{
    public int prepareTime;
    List<SingleAction> actions;
}

[System.Serializable]
public class ActionBar
{
        public Sprite imageAttack;
        public Sprite imageMove;
        public List<SingleAction> actions;
}