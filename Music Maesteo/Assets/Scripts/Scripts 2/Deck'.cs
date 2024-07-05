using UnityEngine;
using System.Collections.Generic;

public class Deck : MonoBehaviour
{
    public List<Card> allCards;
    private List<Card> deck;
    public Transform playerHand;
    public Transform aiHand;
    public GameObject cardPrefab;

    public void ShuffleDeck()
    {
        deck = new List<Card>(allCards);
        for (int i = 0; i < deck.Count; i++)
        {
            Card temp = deck[i];
            int randomIndex = Random.Range(i, deck.Count);
            deck[i] = deck[randomIndex];
            deck[randomIndex] = temp;
        }
        Debug.Log("Deck shuffled");
    }

    public void DistributeInitialCards()
    {
        // Clear previous cards if any
        foreach (Transform child in playerHand)
        {
            Destroy(child.gameObject);
        }

        foreach (Transform child in aiHand)
        {
            Destroy(child.gameObject);
        }

        for (int i = 0; i < 5; i++)
        {
            DrawCard(playerHand);
            DrawCard(aiHand);
        }
        Debug.Log("Initial cards distributed");
    }

    public void DrawCard(Transform hand)
    {
        if (deck.Count == 0)
            return;

        Card drawnCard = deck[0];
        deck.RemoveAt(0);

        GameObject cardGO = Instantiate(cardPrefab, hand);
        cardGO.transform.localScale = Vector3.one; // Ensure default scale
        CardDisplay cardDisplay = cardGO.GetComponent<CardDisplay>();
        if (cardDisplay != null)
        {
            cardDisplay.Setup(drawnCard);
            Debug.Log("Card drawn: " + drawnCard.cardName);
        }
        else
        {
            Debug.LogError("CardDisplay component missing on card prefab");
        }
    }
}