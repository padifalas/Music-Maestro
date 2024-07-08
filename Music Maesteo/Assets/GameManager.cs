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
    public Image playerTurnIndicator; // UI element indicating player's turn
    public Image aiTurnIndicator;     // UI element indicating AI's turn

    public GameObject cardPrefab; // Drag your Card prefab here in the Inspector

    private List<Card> playerHand = new List<Card>();
    private List<Card> aiHand = new List<Card>();
    private int playerScore = 0;
    private int aiScore = 0;
    private bool isPlayerTurn = true;

    private bool gameStarted = false;

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
        shuffleButton.onClick.AddListener(StartGame);
        // Disable turn indicators initially
        playerTurnIndicator.gameObject.SetActive(false);
        aiTurnIndicator.gameObject.SetActive(false);
    }

    void StartGame()
    {
        // Initialize game setup
        DealInitialCards();
        gameStarted = true;
        StartNextTurn();
    }

    void DealInitialCards()
    {
        // Deal 6 cards to player
        playerHand = deckManager.DealCards(6);
        if (playerHand == null)
        {
            Debug.LogError("Failed to deal player cards.");
            return;
        }

        // Deal 6 cards to AI
        aiHand = deckManager.DealCards(6);
        if (aiHand == null)
        {
            Debug.LogError("Failed to deal AI cards.");
            return;
        }

        LayoutPlayerCards(playerHand);
        LayoutAICards(aiHand);
    }

    void LayoutPlayerCards(List<Card> cards)
    {
        float cardWidth = cardPrefab.GetComponent<RectTransform>().rect.width;
        float spacing = 20f; // Adjust this value for spacing between cards
        float startX = -(cards.Count - 1) * (cardWidth / 2 + spacing / 2);
        for (int i = 0; i < cards.Count; i++)
        {
            RectTransform cardTransform = cards[i].GetComponent<RectTransform>();
            cardTransform.SetParent(playerHandPanel);
            cardTransform.localPosition = new Vector3(startX + i * (cardWidth + spacing), 0f, 0f);
            cardTransform.localScale = Vector3.one;
        }
    }

    void LayoutAICards(List<Card> cards)
    {
        float cardWidth = cardPrefab.GetComponent<RectTransform>().rect.width;
        float spacing = 20f; // Adjust this value for spacing between cards
        float startX = (cards.Count - 1) * (cardWidth / 2 + spacing / 2);
        for (int i = 0; i < cards.Count; i++)
        {
            RectTransform cardTransform = cards[i].GetComponent<RectTransform>();
            cardTransform.SetParent(aiHandPanel);
            cardTransform.localPosition = new Vector3(startX - i * (cardWidth + spacing), 0f, 0f);
            cardTransform.localScale = Vector3.one;
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
            else
            {
                aiHand.Remove(card);
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
            StartNextTurn();
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

    void StartNextTurn()
    {
        // Reset turn-specific variables
        isPlayerTurn = !isPlayerTurn;
        UpdateTurnIndicators();

        if (!gameStarted)
            return;

        if (!isPlayerTurn)
        {
            aiManager.TakeTurn();
        }
    }

    void UpdateTurnIndicators()
    {
        if (isPlayerTurn)
        {
            playerTurnIndicator.gameObject.SetActive(true);
            aiTurnIndicator.gameObject.SetActive(false);
        }
        else
        {
            playerTurnIndicator.gameObject.SetActive(false);
            aiTurnIndicator.gameObject.SetActive(true);
        }
    }
}
