using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster : Unit
{
    [SerializeField]

    private Sprite image;
    public MonsterInfo monsterInfo = new();
    public string partToBeHit;

    public int totalHealth;
    public string monsterName;

    public MonsterActionCard[] actionCardsDeck;
    public Stack<MonsterActionCard> shuffledDeck;
    public MonsterActionCard currentActionCard;

    public Dictionary<string, MonsterPartition> bodyParts;
    public MonsterInfo Info { get => monsterInfo; set => monsterInfo = value; }

    private void Awake()
    {
        shuffledDeck = new();
        totalHealth = monsterInfo.totalHealth;
        monsterName = monsterInfo.monsterName;
        bodyParts = monsterInfo.partitions;
    }

    public void TakeDamage(string partName, int value)
    {
        bodyParts[partName].health -= value;
        if (bodyParts[partName].health <= 0)
        {
            bodyParts[partName].health = 0;
            GameManager.Instance.EndGame();
        }
    }

    public void CheckCurrentActionCard()
    {
        //check if current card is completed
        if (currentActionCard != null)
        {
            if (GameManager.Instance.combatManager.CheckCardComplete(currentActionCard))
            {
                DrawAndActivateNewActionCard();
            }
            //End turn if card still acting
            else
            {
                Debug.Log("Preparing for next action");
                GameManager.Instance.combatManager.EndCurrentTurn();
            }
        }
        else 
            DrawAndActivateNewActionCard();
    }

    public MonsterActionCard DrawAndActivateNewActionCard()
    {
        if (shuffledDeck.Count == 0)
        {
            ShuffleCard();
        }

        currentActionCard = shuffledDeck.Pop();
        Debug.Log(currentActionCard == null);
        Debug.Log("get new card from deck,remaining" + shuffledDeck.Count.ToString());
        GameManager.Instance.uiManager.UpdateDeckCount(); // Update UI

        GameManager.Instance.combatManager.Activate(currentActionCard); // Activate the card

        return currentActionCard; // Return the drawn card
    }


    public void React(int value, string partition)
    {
        
    }

    public Stack<MonsterActionCard> ShuffleCards()
    {
        foreach(var card in actionCardsDeck)
        {
            card.Initialize();
        }
        // 创建一个列表来保存牌堆中的牌
        List<MonsterActionCard> cardsList = new List<MonsterActionCard>(actionCardsDeck);
        
        // 使用System.Random来生成随机数
        System.Random rng = new System.Random();
        
        // 从最后一张牌开始，向前遍历牌堆
        int n = cardsList.Count;
        while (n > 1) 
        {
            n--;
            // 随机选择一个元素（介于0和n之间，包括0但不包括n）
            int k = rng.Next(n + 1);
            
            // 交换当前元素和随机选择的元素
            MonsterActionCard value = cardsList[k];
            cardsList[k] = cardsList[n];
            cardsList[n] = value;
        }
        
        // 将洗好的列表转换为栈并返回
        return new Stack<MonsterActionCard>(cardsList);
    }
    public void ShuffleCard()
    {
        Debug.Log("Shuffle cards");
        shuffledDeck = ShuffleCards();
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
    public MonsterPartition lethalPartition = null;
    public Dictionary<string, MonsterPartition> partitions;
    public int toughness = 0;
    public int basicMove;
}

[System.Serializable]
public class MonsterPartition
{
    public string partitionName;
    public int health;
    public float rate = 1.0f;
}

