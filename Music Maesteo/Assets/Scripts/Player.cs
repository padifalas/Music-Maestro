using UnityEngine;
using System.Collections.Generic;

public class Player : MonoBehaviour {
    public string PlayerName;
    public List<Card> Hand;
    private GameManager gameManager;
    public bool isAI;

    void Start() {
        Hand = new List<Card>();
        gameManager = FindObjectOfType<GameManager>();
        isAI = false;
    }

    public void DrawCard(Card card) {
        Hand.Add(card);
    }

    public void PlayCard(Card card) {
        Hand.Remove(card);
        // Additional logic for playing a card
    }

    public void EndTurn() {
        gameManager.EndTurn();
    }

    public void PerformAITurn() {
        if (isAI) {
            Card cardToPlay = ChooseCardToPlay();
            PlayCard(cardToPlay);
            EndTurn();
        }
    }

    private Card ChooseCardToPlay() {
        int randomIndex = Random.Range(0, Hand.Count);
        return Hand[randomIndex];
    }
}