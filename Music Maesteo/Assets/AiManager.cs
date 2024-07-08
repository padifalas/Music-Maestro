using System.Collections.Generic;
using UnityEngine;

public class AIManager : MonoBehaviour
{
    public List<Card> aiHand = new List<Card>();

    public void TakeTurn()
    {
        if (CanComposeSong(out List<Card> songCards))
        {
            PlaySong(songCards);
        }
        else
        {
            // Implement other AI strategies
            // For example: draw a card, discard a card, etc.
        }
    }

    bool CanComposeSong(out List<Card> songCards)
    {
        songCards = new List<Card>();

        List<Card> genreCards = aiHand.FindAll(card => card.cardType == Card.CardType.Genre);
        List<Card> noteCards = aiHand.FindAll(card => card.cardType == Card.CardType.Note);
        List<Card> instrumentCards = aiHand.FindAll(card => card.cardType == Card.CardType.Instrument);
        List<Card> specialCards = aiHand.FindAll(card => card.cardType == Card.CardType.Special);

        if (genreCards.Count > 0 && noteCards.Count >= 4 && instrumentCards.Count > 0 && specialCards.Count > 0)
        {
            songCards.AddRange(noteCards.GetRange(0, 4));
            songCards.Add(genreCards[0]);
            songCards.Add(instrumentCards[0]);
            songCards.Add(specialCards[0]);
            return true;
        }
        return false;
    }

    void PlaySong(List<Card> songCards)
    {
        foreach (var card in songCards)
        {
            aiHand.Remove(card);
            Destroy(card.gameObject);
        }
        GameManager.Instance.OnSongPlayed(songCards, false);
    }
}