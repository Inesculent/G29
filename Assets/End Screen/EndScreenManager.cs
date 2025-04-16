using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class EndingScreenManager : MonoBehaviour
{
    public AudioSource musicSource;
    private void Start()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        if (musicSource != null)
        {
            musicSource.Play();
            musicSource.playOnAwake = true;
            musicSource.loop = true;
        }
    }
    public void ReplayGame()
    {
        Debug.Log("Replay");
        SceneManager.LoadScene("SampleScene");
    }

    public void GoToMainMenu()
    {
        Debug.Log("Menu");
        SceneManager.LoadScene("Menu");
    }
}

