using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster : Unit
{
    [SerializeField]
    private int MonsterId;

    private Sprite image;
    private MonsterInfo info;

    public MonsterActionCard[] actionCardsDeck;

    public MonsterInfo Info { get => info; set => info = value; }


    void Start()
    {
        
    }

    void Update()
    {
        
    }
}

[System.Serializable]
public class MonsterInfo
{
    public string descrption;
    public bool isEpic;
    [Header("Combat info")]
    public int totalHealth;
    public int deathHealth;
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

