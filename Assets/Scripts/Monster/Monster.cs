using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;

public class Monster : Unit
{
    [SerializeField]

    private Sprite image;
    public MonsterInfo monsterInfo;
    [HideInInspector]
    public string partToBeHit;
    [HideInInspector]
    public int totalHealth;
    [HideInInspector]
    public int toughness;
    [Header("State ui")]
    public TextMeshProUGUI stateText;
    public GameObject stateBackground;

    public MonsterActionCard[] actionCardsDeck;
    public Stack<MonsterActionCard> shuffledDeck;

    [HideInInspector]
    public MonsterActionCard currentActionCard;

    public Dictionary<string, MonsterPartition> bodyParts = new();
    public MonsterInfo Info { get => monsterInfo; set => monsterInfo = value; }
    public int TotalHealth { get => GetTotalHealth(); }

    public BodyPartGraph bodyPartGraph;

    private void Awake()
    {
        shuffledDeck = new();
        toughness = monsterInfo.toughness;
        totalHealth = monsterInfo.totalHealth;
        unitName = monsterInfo.monsterName;
        foreach(var part in monsterInfo.partitions)
        {
            bodyParts.Add(part.partitionName, new MonsterPartition(part.partitionName, part.health));
        }
    }

    public void TakeDamage(string partName, int value)
    {
        bodyParts[partName].health -= value;
        GameManager.Instance.uiManager.monsterInfo.UpdateInfo(this);

        if(bodyParts[partName].health <= 0)
        {
            bodyPartGraph.DisableButton(partName);
            bodyParts[partName].health = 0;
        }
        if (GetTotalHealth() < monsterInfo.deathThreshold)
        {
            Debug.Log("Monster dies");
            GameManager.Instance.EndGame();
        }
    }

    private int GetTotalHealth()
    {
        int total = 0;
        foreach(var entry in bodyParts)
        {
            total += entry.Value.health;
        }
        return total;
    }

    public async Task CheckCurrentActionCardAsync()
    {
        //check if current card is completed
        if (currentActionCard != null)
        {
            if (GameManager.Instance.combatManager.CheckCardComplete(currentActionCard))
            {
                //new new card
                DrawAndActivateNewActionCard();
            }
            //End turn if card still acting
            else
            {
                await ShowStateText("Preparing for next action");
                GameManager.Instance.combatManager.EndCurrentTurn();
            }
        }
        else 
            DrawAndActivateNewActionCard();
    }

    internal async Task ShowStateText(string content, int ms = 1500)
    {
        stateBackground.SetActive(true);
        stateBackground.GetComponent<RectTransform>().sizeDelta = new(0.5f * content.Length, 1);
        stateText.gameObject.SetActive(true);
        stateText.text = content;
        await Task.Delay(ms);
        stateText.gameObject.SetActive(false);
        stateBackground.SetActive(false);
    }

    public MonsterActionCard DrawAndActivateNewActionCard()
    {
        if (shuffledDeck.Count == 0)
        {
            ShuffleCard();
        }

        currentActionCard = shuffledDeck.Pop();
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
public class MonsterPartition
{
    public string partitionName;
    public int health;
    public float rate = 1.0f;

    public MonsterPartition(string partitionName, int health)
    {
        this.partitionName = partitionName;
        this.health = health;
    }
}

