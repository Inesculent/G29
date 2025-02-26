using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;
    public AudioMixer audioMixer; // Reference to Unity Audio Mixer
    private AudioSource backgroundMusic;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }
        
        // Find the Background Music Player in the scene
        backgroundMusic = GameObject.Find("BackgroundMusicPlayer")?.GetComponent<AudioSource>();
    }

    private void Start()
    {
        LoadVolumeSettings();
    }

    public void SetMusicVolume(float volume)
    {
        audioMixer.SetFloat("MusicVolume", Mathf.Log10(volume) * 20);
        PlayerPrefs.SetFloat("MusicVolume", volume);

        if (backgroundMusic != null)
        {
            backgroundMusic.volume = volume; // Apply volume directly
        }
    }

    public void SetSFXVolume(float volume)
    {
        audioMixer.SetFloat("SFXVolume", Mathf.Log10(volume) * 20);
        PlayerPrefs.SetFloat("SFXVolume", volume);
    }

    public void LoadVolumeSettings()
    {
        float musicVol = PlayerPrefs.GetFloat("MusicVolume", 0.5f);
        float sfxVol = PlayerPrefs.GetFloat("SFXVolume", 0.5f);
        SetMusicVolume(musicVol);
        SetSFXVolume(sfxVol);
    }

    public void StopMusic()
    {
        if (backgroundMusic != null)
        {
            backgroundMusic.Stop();
        }
    }

    public void PlayMusic()
    {
        if (backgroundMusic != null && !backgroundMusic.isPlaying)
        {
            backgroundMusic.Play();
        }
    }
}
