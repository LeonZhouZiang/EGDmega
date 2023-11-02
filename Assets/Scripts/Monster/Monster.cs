using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster : Unit
{
    [SerializeField]
    private int MonsterId;

    private Sprite image;
    private MonsterInfo monsterInfo = new();

    public int totalHealth;
    public string monsterName;

    public MonsterActionCard[] actionCardsDeck;
    public Stack<MonsterActionCard> shuffledDeck;
    public MonsterActionCard currentActionCard;

    public MonsterInfo Info { get => monsterInfo; set => monsterInfo = value; }

    private void Awake()
    {
        totalHealth = monsterInfo.totalHealth;
        monsterName = monsterInfo.monsterName;
    }

    void Update()
    {
        
    }

    public void CheckCurrentActionCard()
    {
        if(currentActionCard == null) DrawNewActionCard();
        else
        {
            GameManager.Instance.combatManager.Activate(currentActionCard);
        }
    }

    public void DrawNewActionCard()
    {
        if(shuffledDeck.Count == 0)
        {
            shuffledDeck = ShuffleCards();
        }

        currentActionCard = shuffledDeck.Pop();

        GameManager.Instance.uiManager.ShuffleCards();
        GameManager.Instance.combatManager.Activate(currentActionCard);
    }

    public Stack<MonsterActionCard> ShuffleCards()
    {
        Stack<MonsterActionCard> newDeck = new();
        foreach(var card in actionCardsDeck)
        {
            newDeck.Push(card);
        }

        GameManager.Instance.uiManager.ShuffleCards();

        return newDeck;
    }
}

[System.Serializable]
public class MonsterInfo
{
    public string monsterName;
    public string descrption;
    public bool isEpic;
    [Header("Combat info")]
    public int totalHealth;
    public int deathThreshold;
    public Partition lethalPartition = null;
    public List<Partition> partitions;
    public int toughness = 0;
    public int basicMove;
}

[System.Serializable]
public class Partition
{
    public string partitionName;
    public int health;
    public float rate = 1.0f;
}

