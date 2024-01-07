using UnityEngine;
using UnityEngine.SceneManagement;

public class StartGameButton : MonoBehaviour
{
    public void LoadStartGame()
    {
        // Find the MenuManager instance in the scene
        MenuManager menuManager = FindObjectOfType<MenuManager>();
        if (menuManager != null)
        {
            menuManager.StartGame();
        }
        else
        {
            Debug.LogError("MenuManager instance not found in the scene.");
        }
    }
}
