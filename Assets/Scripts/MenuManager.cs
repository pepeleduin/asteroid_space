using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    public TMP_InputField NameBox;

    public void StartGame()
    {
        if (NameBox != null)
        {
            // Save the player's name in PlayerPrefs
            PlayerPrefs.SetString("PlayerName", NameBox.text);
        }
        
        // Load the game scene
        SceneManager.LoadScene("MainScene"); 
    }
}