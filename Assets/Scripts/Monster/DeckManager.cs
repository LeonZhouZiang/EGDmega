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
        }
    }
     private void DrawNewActionCard()
    {
        if (monster.shuffledDeck.Count > 0)
        {
            MonsterActionCard cardData = monster.DrawNewActionCard();

            // Instantiate the card game object
            GameObject cardObject = Instantiate(cardPrefab, this.transform.position, Quaternion.identity);
            cardObject.transform.localScale = new Vector3(0.3f, 0.3f, 0.3f);
         

            // Set the card data on the newly created card object
            ActionCardDisplay cardDisplay = cardObject.GetComponent<ActionCardDisplay>();
            cardDisplay.DisplayCard(cardData);

            // Start coroutine to move the card to the draw point
            StartCoroutine(MoveCardToPosition(cardObject, drawPoint.position, 1f));
            
        }
        else
        {
            Debug.Log("Deck is empty, shuffling...");
            monster.ShuffleCard(); // Shuffle the deck if empty before drawing a card
            if (currentDisplayedCard != null)
            {
                Destroy(currentDisplayedCard);
            }
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
}

