using UnityEngine;
using TMPro;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance { get; private set; }
    public TextMeshPro scoreText; 
    private int score;
    private int maxScore = 0;
    public int LatestScore { get; private set; }
    public string PlayerName { get; private set; }
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);

            // Retrieve the player's name from PlayerPrefs
            PlayerName = PlayerPrefs.GetString("PlayerName", "PLAYER1");
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        // Find the TextMeshPro object in the scene
        if (scoreText == null)
        {
            scoreText = FindObjectOfType<TextMeshPro>();
        }
        UpdateScoreText(); // Update score text at start to ensure it displays the current score
    }

    public void SetPlayerName(string name)
    {
        PlayerName = name;
    }

    public void IncreaseScore(int amount)
    {
        score += amount;
        UpdateScoreText();
    }

    public void CheckMaxScore()
    {
        if(score > maxScore){
            maxScore = score;
            LatestScore = maxScore;
        }
    }

    public void ResetScore()
    {
        CheckMaxScore();
        score = 0;
        UpdateScoreText();
    }

    private void UpdateScoreText()
    {
        Debug.Log("Score: " + score);
        
        if (scoreText == null)
        {
            scoreText = FindObjectOfType<TextMeshPro>();
        }

        if (scoreText != null)
        {
            scoreText.text = "Score: " + score;
        }
    }
}