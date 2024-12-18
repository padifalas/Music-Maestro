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
    public Transform discardPilePanel;
    public Button shuffleButton;
    public Button drawButton;
    public Button discardButton; // Add discard button
    public TMP_Text playerScoreText;
    public TMP_Text aiScoreText;
    public Image playerTurnIndicator;
    public Image aiTurnIndicator;
    public GameObject cardPrefab;
    public GameObject discardPromptPanel;

    private List<Card> playerHand = new List<Card>();
    private List<Card> aiHand = new List<Card>();
    private List<Card> deck = new List<Card>();
    private List<Card> discardPile = new List<Card>();
    private int playerScore = 0;
    private int aiScore = 0;
    private bool isPlayerTurn = true;

    private bool gameStarted = false;
    private bool shuffleButtonClicked = false;
    private bool isDiscardPromptActive = false;
    private Card selectedCardToDiscard = null; // Track selected card to discard

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
        drawButton.onClick.AddListener(DrawCard);
        discardButton.onClick.AddListener(OnDiscardButtonClicked); // Add listener for discard button
        playerTurnIndicator.gameObject.SetActive(false);
        aiTurnIndicator.gameObject.SetActive(false);
        drawButton.interactable = false;
        discardButton.interactable = false; // Initially disable discard button
        discardPromptPanel.SetActive(false);
    }

    void StartGame()
    {
        DealInitialCards();
        SetupDeck();
        gameStarted = true;
        StartNextTurn();
        drawButton.interactable = shuffleButtonClicked;
    }

    void SetupDeck()
    {
        deckManager.InitializeDeck();
        deck = new List<Card>(deckManager.deck);
        ShuffleDeck();
    }

    void ShuffleDeck()
    {
        for (int i = 0; i < deck.Count; i++)
        {
            int randomIndex = Random.Range(i, deck.Count);
            Card temp = deck[randomIndex];
            deck[randomIndex] = deck[i];
            deck[i] = temp;
        }
        shuffleButtonClicked = true;
        drawButton.interactable = true;
    }

    void DealInitialCards()
    {
        playerHand = deckManager.DealCards(6);
        if (playerHand == null)
        {
            Debug.LogError("Failed to deal player cards.");
            return;
        }

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
        foreach (Transform child in playerHandPanel)
        {
            Destroy(child.gameObject);
        }

        float cardWidth = cardPrefab.GetComponent<RectTransform>().rect.width;
        float spacing = 20f;
        float startX = -(cards.Count - 1) * (cardWidth / 2 + spacing / 2);
        for (int i = 0; i < cards.Count; i++)
        {
            Card card = Instantiate(cardPrefab, playerHandPanel).GetComponent<Card>();
            card.Initialize(cards[i].cardType, cards[i].cardValue, cards[i].cardSprite);
            RectTransform cardTransform = card.GetComponent<RectTransform>();
            cardTransform.localPosition = new Vector3(startX + i * (cardWidth + spacing), 0f, 0f);
            cardTransform.localScale = Vector3.one;
            card.GetComponent<Button>().onClick.AddListener(() => OnCardSelectedForDiscard(card)); // Add listener for card selection
        }
    }

    void LayoutAICards(List<Card> cards)
    {
        foreach (Transform child in aiHandPanel)
        {
            Destroy(child.gameObject);
        }

        float cardWidth = cardPrefab.GetComponent<RectTransform>().rect.width;
        float spacing = 20f;
        float startX = (cards.Count - 1) * (cardWidth / 2 + spacing / 2);
        for (int i = 0; i < cards.Count; i++)
        {
            Card card = Instantiate(cardPrefab, aiHandPanel).GetComponent<Card>();
            card.Initialize(cards[i].cardType, cards[i].cardValue, cards[i].cardSprite);
            RectTransform cardTransform = card.GetComponent<RectTransform>();
            cardTransform.localPosition = new Vector3(startX - i * (cardWidth + spacing), 0f, 0f);
            cardTransform.localScale = Vector3.one;
        }
    }

    void LayoutDiscardPile()
    {
        foreach (Transform child in discardPilePanel)
        {
            Destroy(child.gameObject);
        }

        float offset = 5f;
        for (int i = 0; i < discardPile.Count; i++)
        {
            Card discardedCard = Instantiate(cardPrefab, discardPilePanel).GetComponent<Card>();
            discardedCard.Initialize(discardPile[i].cardType, discardPile[i].cardValue, discardPile[i].cardSprite);
            RectTransform cardTransform = discardedCard.GetComponent<RectTransform>();
            cardTransform.localPosition = new Vector3(0, -i * offset, 0);
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

    void DrawCard()
    {
        if (deck.Count > 0)
        {
            Card drawnCard = deck[0];
            deck.RemoveAt(0);
            playerHand.Add(drawnCard);
            LayoutPlayerCards(playerHand);
            discardButton.interactable = true; // Enable discard button after drawing a card
        }
        else
        {
            Debug.Log("No cards left in the deck.");
        }
    }

    public void OnCardSelectedForDiscard(Card card)
    {
        selectedCardToDiscard = card;
    }

    public void OnDiscardButtonClicked()
    {
        if (selectedCardToDiscard != null)
        {
            playerHand.Remove(selectedCardToDiscard);
            discardPile.Add(selectedCardToDiscard);
            LayoutDiscardPile();
            LayoutPlayerCards(playerHand);
            discardButton.interactable = false; // Disable discard button after discarding a card
            selectedCardToDiscard = null;
        }
        else
        {
            Debug.Log("No card selected to discard.");
        }
    }
}
