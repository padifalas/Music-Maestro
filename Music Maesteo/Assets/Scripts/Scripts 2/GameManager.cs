using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public Deck deck;
    public Button shuffleButton;
    public Player player;
    public AIPlayer aiPlayer;

    private void Awake()
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
        shuffleButton.onClick.AddListener(OnShuffleButtonClicked);
    }

    private void OnShuffleButtonClicked()
    {
        deck.ShuffleDeck();
        deck.DistributeInitialCards();
        StartGame();
    }

    public void StartGame()
    {
        PlayerTurn();
    }

    public void PlayerTurn()
    {
        player.StartTurn();
    }

    public void AITurn()
    {
        aiPlayer.StartTurn();
    }
}