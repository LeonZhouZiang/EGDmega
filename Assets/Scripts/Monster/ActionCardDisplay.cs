using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class ActionCardDisplay : MonoBehaviour
{
    public MonsterActionCard cardData;
    public Image cardImage;
    public TMP_Text cardNameText;
    public TMP_Text cardDescriptionText;

    public void DisplayCard(MonsterActionCard cardData)
    {
        cardImage.sprite = cardData.image;
        cardNameText.text = cardData.cardName;
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
