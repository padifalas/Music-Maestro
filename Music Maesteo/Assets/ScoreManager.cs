using UnityEngine;
using TMPro;

public class ScoreManager : MonoBehaviour
{
    public TMP_Text playerScoreText;
    public TMP_Text aiScoreText;

    private int playerScore = 0;
    private int aiScore = 0;

    public void AddPlayerScore(int score)
    {
        playerScore += score;
        UpdateScoreUI();
    }

    public void AddAIScore(int score)
    {
        aiScore += score;
        UpdateScoreUI();
    }

    void UpdateScoreUI()
    {
        playerScoreText.text = "Player Score: " + playerScore;
        aiScoreText.text = "AI Score: " + aiScore;
    }
}