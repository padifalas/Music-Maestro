using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public DeckManager deckManager;
    public AIManager aiManager;
    public Transform playerHandPanel;
    public Transform aiHandPanel;
    public Button shuffleButton;
    public TMP_Text playerScoreText;
    public TMP_Text aiScoreText;

    private List<Card> playerHand = new List<Card>();
    private int playerScore = 0;
    private int aiScore = 0;
    private bool isPlayerTurn = true;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        shuffleButton.onClick.AddListener(DealInitialCards);
    }

    void DealInitialCards()
    {
        playerHand = deckManager.DealCards(6);
        if (playerHand == null)
        {
            Debug.LogError("Failed to deal player cards.");
            return;
        }

        List<Card> aiHand = deckManager.DealCards(6);
        if (aiHand == null)
        {
            Debug.LogError("Failed to deal AI cards.");
            return;
        }

        foreach (var card in playerHand)
        {
            card.transform.SetParent(playerHandPanel);
        }

        aiManager.aiHand.AddRange(aiHand);
        foreach (var card in aiHand)
        {
            card.transform.SetParent(aiHandPanel);
        }
    }

    public void OnCardClicked(Card card)
    {
        if (isPlayerTurn && playerHand.Contains(card))
        {
            if (CanComposeSong(playerHand, out List<Card> songCards))
            {
                PlaySong(songCards, true);
            }
        }
    }

    bool CanComposeSong(List<Card> hand, out List<Card> songCards)
    {
        songCards = new List<Card>();

        List<Card> genreCards = hand.FindAll(card => card.cardType == Card.CardType.Genre);
        List<Card> noteCards = hand.FindAll(card => card.cardType == Card.CardType.Note);
        List<Card> instrumentCards = hand.FindAll(card => card.cardType == Card.CardType.Instrument);
        List<Card> specialCards = hand.FindAll(card => card.cardType == Card.CardType.Special);

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

    void PlaySong(List<Card> songCards, bool isPlayer)
    {
        foreach (var card in songCards)
        {
            if (isPlayer)
            {
                playerHand.Remove(card);
            }
            Destroy(card.gameObject);
        }
        OnSongPlayed(songCards, isPlayer);
    }

    public void OnSongPlayed(List<Card> songCards, bool isPlayer)
    {
        if (isPlayer)
        {
            playerScore++;
            playerScoreText.text = "Player Score: " + playerScore;
        }
        else
        {
            aiScore++;
            aiScoreText.text = "AI Score: " + aiScore;
        }

        if (CheckWinCondition())
        {
            EndGame();
        }
        else
        {
            isPlayerTurn = !isPlayerTurn;
            if (!isPlayerTurn)
            {
                aiManager.TakeTurn();
            }
        }
    }

    bool CheckWinCondition()
    {
        return playerScore >= 5 || aiScore >= 5;
    }

    void EndGame()
    {
        // Implement end game logic (e.g., display a win/loss message)
    }
}
