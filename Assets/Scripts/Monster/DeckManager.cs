using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeckManager : MonoBehaviour
{
    public Monster monster; // 从 Inspector 中设置
    public UIManager uiManager; // 从 Inspector 中设置
    public GameObject cardPrefab; // 从 Inspector 中设置
    public Transform cardParent; // 从 Inspector 中设置
    public Transform drawPoint;
    private GameObject currentDisplayedCard;
    
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.K))
        {
            DrawNewActionCard();
        }
        if (Input.GetKeyDown(KeyCode.L))
        {
            monster.ShuffleCards(); // Shuffle the deck
            StartCoroutine(ShuffleDeckVisual()); // Trigger the shuffle animation
        }
    }
     private void DrawNewActionCard()
    {
        if (monster.shuffledDeck.Count > 0)
        {
            MonsterActionCard cardData = monster.DrawAndActivateNewActionCard();

            // Instantiate the card game object
            GameObject cardObject = Instantiate(cardPrefab, this.transform.position, Quaternion.identity);
            cardObject.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
            //cardObject.transform.SetParent(cardParent, false); // 确保牌的父对象是cardParent

            // Set the card data on the newly created card object
            ActionCardDisplay cardDisplay = cardObject.GetComponent<ActionCardDisplay>();
            cardDisplay.DisplayCard(cardData);

            // Start coroutine to move the card to the draw point
            StartCoroutine(MoveCardToPosition(cardObject, drawPoint.position, 1f));
            
        }
        else
        {
            if (currentDisplayedCard != null)
            {
                Destroy(currentDisplayedCard);
            }
            Debug.Log("Deck is empty, shuffling...");
            monster.ShuffleCard(); // 洗牌逻辑
            StartCoroutine(ShuffleDeckVisual()); // 触发洗牌动画
            
        }
    }

    private IEnumerator MoveCardToPosition(GameObject card, Vector3 newPosition, float timeToMove)
    {
        var currentPos = card.transform.position;
        var t = 0f;
        while (t < 1)
        {
            t += Time.deltaTime / timeToMove;
            card.transform.position = Vector3.Lerp(currentPos, newPosition, t);
            yield return null;
        }
        if (currentDisplayedCard != null)
        {
            Destroy(currentDisplayedCard);
        }
        currentDisplayedCard = card;
    }
    private IEnumerator ShuffleDeckVisual()
    {
        GameObject cardBack1 = Instantiate(cardPrefab, drawPoint.position, Quaternion.identity);
        cardBack1.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f); // 设置卡片的大小
        //rotate Z 180
        cardBack1.transform.Rotate(0, 0, 180);
        for (int i = 0; i < monster.actionCardsDeck.Length - 1; i++)
        {
            GameObject cardBack = Instantiate(cardPrefab, drawPoint.position, Quaternion.identity);
            cardBack.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f); // 设置卡片的大小
            //rotate Z 180
            cardBack.transform.Rotate(0, 0, 180);
            // cardBack.transform.SetParent(cardParent, false); // 如果需要，可以取消注释

            // Move each card to the deck one by one with a delay between them
            yield return StartCoroutine(MoveCardToDeck(cardBack, cardParent.position, 0.4f));
            
            // Wait for a short delay before moving the next card
            yield return new WaitForSeconds(0.2f);
        }
        yield return StartCoroutine(MoveCardToDeck(cardBack1, cardParent.position, 0.4f));
        Destroy(cardBack1);
    }

    private IEnumerator MoveCardToDeck(GameObject card, Vector3 newPosition, float timeToMove)
    {
        var currentPos = card.transform.position;
        var t = 0f;
        while (t < 1)
        {
            t += Time.deltaTime / timeToMove;
            card.transform.position = Vector3.Lerp(currentPos, newPosition, t);
            yield return null;
        }
        Destroy(card); // Destroy the card after it has moved to the deck
    }
    
}
