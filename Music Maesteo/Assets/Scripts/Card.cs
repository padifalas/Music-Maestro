using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro; 

public enum CardType {
    Note,
    Instrument,
    Genre,
    SpecialAction
}

public class Card : MonoBehaviour {
    public CardType Type;
    public string CardName;
    public Sprite CardImage;

    // References to UI components
    public Image cardImageUI;
    public TMP_Text cardNameUI;

    void Start() {
        // Assign the image and text
        if (cardImageUI != null) {
            cardImageUI.sprite = CardImage;
        }
        if (cardNameUI != null) {
            cardNameUI.text = CardName;
        }
    }
}
