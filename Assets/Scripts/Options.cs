using UnityEngine;
using UnityEngine.UI;

public class Options : MonoBehaviour
{
    public Slider musicSlider;
    public Slider sfxSlider;

    private void Start()
    {
        if (AudioManager.Instance != null)
        {
            // Load saved volume settings from AudioManager
            musicSlider.value = PlayerPrefs.GetFloat("MusicVolume", 0.5f);
            sfxSlider.value = PlayerPrefs.GetFloat("SFXVolume", 0.5f);

            // Add listener for real-time changes
            musicSlider.onValueChanged.AddListener(AudioManager.Instance.SetMusicVolume);
            sfxSlider.onValueChanged.AddListener(AudioManager.Instance.SetSFXVolume);
        }
    }
}
