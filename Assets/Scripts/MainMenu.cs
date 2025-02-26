
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void StartGame()
    {
        SceneManager.LoadScene("GameScene"); // Change to your game scene name
    }

    public void OpenOptions()
    {
        Debug.Log("Options Menu Opened"); // Replace with options menu logic
    }

    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("Game Quit"); // Only visible in editor
    }
}

