using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MonsterInfo", menuName = "MonsterInfo")]
public class MonsterInfo : ScriptableObject
{
    public string monsterName;
    public string descrption;
    public bool isEpic;
    [Header("Combat info")]
    public int totalHealth;
    public int deathThreshold;
    public string lethalPartition = null;
    [SerializeField]
    public List<MonsterPartition> partitions;
    public int toughness = 0;
    public int basicMove;
}
