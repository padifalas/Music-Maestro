using UnityEngine;

public class AIPlayer : MonoBehaviour
{
    public Transform hand;
    public Deck deck;

    public void StartTurn()
    {
        // AI's turn logic
        Debug.Log("AI's turn started");
        DrawCard();
        // Implement AI logic to decide which card to play
        EndTurn();
    }

    public void DrawCard()
    {
        deck.DrawCard(hand);
    }

    public void PlayCard(Card card)
    {
        // Implement the logic to play a card
        Debug.Log("AI played card: " + card.cardName);
    }

    public void EndTurn()
    {
        // End turn logic
        Debug.Log("AI's turn ended");
        GameManager.Instance.PlayerTurn();
    }
}