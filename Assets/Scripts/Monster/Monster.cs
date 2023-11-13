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
        // ����һ���б��������ƶ��е���
        List<MonsterActionCard> cardsList = new List<MonsterActionCard>(actionCardsDeck);
        
        // ʹ��System.Random�����������
        System.Random rng = new System.Random();
        
        // �����һ���ƿ�ʼ����ǰ�����ƶ�
        int n = cardsList.Count;
        while (n > 1) 
        {
            n--;
            // ���ѡ��һ��Ԫ�أ�����0��n֮�䣬����0��������n��
            int k = rng.Next(n + 1);
            
            // ������ǰԪ�غ����ѡ���Ԫ��
            MonsterActionCard value = cardsList[k];
            cardsList[k] = cardsList[n];
            cardsList[n] = value;
        }
        
        // ��ϴ�õ��б�ת��Ϊջ������
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

