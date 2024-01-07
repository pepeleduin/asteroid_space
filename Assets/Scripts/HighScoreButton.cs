using UnityEngine;
using UnityEngine.SceneManagement;

public class HighScoreButton : MonoBehaviour
{
    public void LoadHighScoreTable()
    {
        SceneManager.LoadScene("ScoreRanking"); 
        Debug.Log("Load high score table");
    }
}