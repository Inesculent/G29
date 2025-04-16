using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private GameObject mainMenu;    // Assign the Main Menu panel in the Inspector
    [SerializeField] private GameObject optionsMenu; // Assign the Options Menu panel in the Inspector
    [SerializeField] private AudioSource buttonClickSound; // Assign a button click sound effect (optional)
    [SerializeField] private string gameSceneName = "SampleScene"; // Set the name of the game scene in the Inspector

    private void Start()
    {
        // Ensure only the main menu is visible at the start
        mainMenu.SetActive(true);
        optionsMenu.SetActive(false);
    }

    public void StartGame()
    {
        if (buttonClickSound != null) buttonClickSound.Play(); // Play button sound (optional)
        SceneManager.LoadScene(gameSceneName);
    }

    public void OpenOptions()
    {
        if (buttonClickSound != null) buttonClickSound.Play(); // Play button sound (optional)

        if (optionsMenu != null && mainMenu != null)
        {
            optionsMenu.SetActive(true);  // Show the Options Menu
            mainMenu.SetActive(false);    // Hide the Main Menu
        }
        else
        {
            Debug.LogWarning("Main Menu or Options Menu is not assigned in the Inspector.");
        }
    }

    public void CloseOptions()
    {
        if (buttonClickSound != null) buttonClickSound.Play(); // Play button sound (optional)

        if (optionsMenu != null && mainMenu != null)
        {
            optionsMenu.SetActive(false); // Hide the Options Menu
            mainMenu.SetActive(true);     // Show the Main Menu
        }
        else
        {
            Debug.LogWarning("Main Menu or Options Menu is not assigned in the Inspector.");
        }
    }

    public void QuitGame()
    {
        if (buttonClickSound != null) buttonClickSound.Play(); // Play button sound (optional)

        Debug.Log("Game Quit");

        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false; // Stops play mode in the Unity Editor
        #else
            Application.Quit(); // Quits the game in a built application
        #endif
    }
}
