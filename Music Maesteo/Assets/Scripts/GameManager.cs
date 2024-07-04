using UnityEngine;

public class GameManager : MonoBehaviour {
    public Player humanPlayer;
    public Player aiPlayer;
    public Deck deck;
    private Player currentPlayer;

    void Start() {
        StartGame();
    }

    void StartGame() {
        DealInitialCards();
        // Determine starting player
        currentPlayer = humanPlayer;
        StartPlayerTurn(currentPlayer);
    }

    void DealInitialCards() {
        for (int i = 0; i < 5; i++) {
            humanPlayer.DrawCard(deck.DrawCard());
            aiPlayer.DrawCard(deck.DrawCard());
        }
    }

    void StartPlayerTurn(Player player) {
        currentPlayer = player;
        if (player.isAI) {
            aiPlayer.PerformAITurn();
        } else {
            // Handle human player's turn (e.g., enable UI for human interaction)
        }
    }

    public void EndTurn() {
        // Switch to the next player's turn
        currentPlayer = (currentPlayer == humanPlayer) ? aiPlayer : humanPlayer;
        StartPlayerTurn(currentPlayer);
    }

    // Additional game management methods can be added here
}