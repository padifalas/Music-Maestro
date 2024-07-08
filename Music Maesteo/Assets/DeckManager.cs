using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DeckManager : MonoBehaviour
{
    public List<Sprite> genreSprites;
    public List<Sprite> noteSprites;
    public List<Sprite> instrumentSprites;
    public List<Sprite> specialSprites;

    private List<Card> deck = new List<Card>();

    void Start()
    {
        InitializeDeck();
    }

    void InitializeDeck()
    {
        AddCardsToDeck(Card.CardType.Genre, genreSprites, 3); // 15 Genre Cards (3 of each type)
        AddCardsToDeck(Card.CardType.Note, noteSprites, 4); // 28 Note Cards (4 of each type)
        AddCardsToDeck(Card.CardType.Instrument, instrumentSprites, 3); // 12 Instrument Cards (3 of each type)
        AddCardsToDeck(Card.CardType.Special, specialSprites, 1); // 4 Special Cards
        ShuffleDeck();
    }

    void AddCardsToDeck(Card.CardType type, List<Sprite> sprites, int count)
    {
        foreach (var sprite in sprites)
        {
            for (int i = 0; i < count; i++)
            {
                deck.Add(CreateCard(type, sprite.name, sprite));
            }
        }
    }

    Card CreateCard(Card.CardType type, string value, Sprite sprite)
    {
        GameObject cardObj = new GameObject("Card");
        cardObj.AddComponent<Image>().sprite = sprite;
        Card card = cardObj.AddComponent<Card>();
        card.Initialize(type, value, sprite);
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
