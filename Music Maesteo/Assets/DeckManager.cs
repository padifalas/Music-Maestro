using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DeckManager : MonoBehaviour
{
    public List<Sprite> genreSprites;
    public List<Sprite> noteSprites;
    public List<Sprite> instrumentSprites;
    public List<Sprite> specialSprites;

    public List<Card> deck = new List<Card>();

    void Start()
    {
        InitializeDeck();
    }

    public void InitializeDeck()
    {
        CreateCards(Card.CardType.Genre, genreSprites, 15);       // 15 Genre Cards
        CreateCards(Card.CardType.Note, noteSprites, 28);         // 28 Note Cards
        CreateCards(Card.CardType.Instrument, instrumentSprites, 12); // 12 Instrument Cards
        CreateCards(Card.CardType.Special, specialSprites, 4);    // 4 Special Cards
        ShuffleDeck();
    }

    void CreateCards(Card.CardType type, List<Sprite> sprites, int count)
    {
        foreach (var sprite in sprites)
        {
            for (int i = 0; i < count; i++)
            {
                Card newCard = CreateCard(type, sprite.name, sprite);
                deck.Add(newCard);
            }
        }
    }

    Card CreateCard(Card.CardType type, string value, Sprite sprite)
    {
        GameObject cardObj = new GameObject("Card");
        Image image = cardObj.AddComponent<Image>();
        image.sprite = sprite;

        // Ensure the card object has a Card component attached
        Card card = cardObj.AddComponent<Card>();
        card.Initialize(type, value, sprite);

        // Make sure the card object is not destroyed when a new scene loads
        DontDestroyOnLoad(cardObj);

        return card;
    }

    void ShuffleDeck()
    {
        for (int i = 0; i < deck.Count; i++)
        {
            Card temp = deck[i];
            int randomIndex = Random.Range(i, deck.Count);
            deck[i] = deck[randomIndex];
            deck[randomIndex] = temp;
        }
    }

    public List<Card> DealCards(int count)
    {
        if (deck.Count < count)
        {
            Debug.LogError("Not enough cards in the deck to deal.");
            return null;
        }

        List<Card> hand = new List<Card>();
        for (int i = 0; i < count; i++)
        {
            hand.Add(deck[0]);
            deck.RemoveAt(0);
        }
        return hand;
    }
}
