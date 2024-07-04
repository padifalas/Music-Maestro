using UnityEngine;
using System.Collections.Generic;

public class Deck : MonoBehaviour {
    public List<Card> Cards;

    void Start() {
        ShuffleDeck();
    }

    public void ShuffleDeck() {
        for (int i = 0; i < Cards.Count; i++) {
            Card temp = Cards[i];
            int randomIndex = Random.Range(i, Cards.Count);
            Cards[i] = Cards[randomIndex];
            Cards[randomIndex] = temp;
        }
    }

    public Card DrawCard() {
        if (Cards.Count == 0) return null;
        Card drawnCard = Cards[0];
        Cards.RemoveAt(0);
        return drawnCard;
    }
}