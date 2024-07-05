using UnityEngine;

public class Player : MonoBehaviour
{
    public Transform hand;
    public Deck deck;

    public void StartTurn()
    {
        // Player's turn logic
        Debug.Log("Player's turn started");
        DrawCard();
    }

    public void DrawCard()
    {
        deck.DrawCard(hand);
    }

    public void PlayCard(Card card)
    {
        // Implement the logic to play a card
        Debug.Log("Player played card: " + card.cardName);
    }

    public void EndTurn()
    {
        // End turn logic
        Debug.Log("Player's turn ended");
        GameManager.Instance.AITurn();
    }
}