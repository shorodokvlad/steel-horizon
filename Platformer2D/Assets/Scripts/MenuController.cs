using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Audio;
using TMPro;


public class MainMenu : MonoBehaviour
{
    [Header("Volume Settings")]
    [SerializeField] private TextMeshProUGUI volumeTextValue = null;
    [SerializeField] private TextMeshProUGUI musicTextValue = null;
    [SerializeField] private TextMeshProUGUI sfxTextValue = null;
    [SerializeField] private float defaultVolume = 0.5f;

    [Header("Audio Mixer")]
    [SerializeField] private AudioMixer audioMixer;
    [Header("Audio Sliders")]
    [SerializeField] private Slider masterSlider;
    [SerializeField] private Slider musicSlider;
    [SerializeField] private Slider sfxSlider;

    [Header("Fullscreen Settings")]
    [SerializeField] private Image fullscreenButtonImage;
    [SerializeField] private Sprite windowedSprite;
    [SerializeField] private Sprite fullscreenSprite;

    private bool isFullscreen;

    void Start()
    {
        isFullscreen = Screen.fullScreen;
        UpdateSprite();

        // Load saved volume or use default if none exists
        float savedMaster = PlayerPrefs.GetFloat("MasterVolume", defaultVolume);
        float savedMusic = PlayerPrefs.GetFloat("MusicVolume", defaultVolume);
        float savedSFX = PlayerPrefs.GetFloat("SFXVolume", defaultVolume);

        // Update Sliders
        masterSlider.value = savedMaster;
        musicSlider.value = savedMusic;
        sfxSlider.value = savedSFX;

        // Apply to Mixer
        SetMasterVolume(savedMaster);
        SetMusicVolume(savedMusic);
        SetSFXVolume(savedSFX);
    }

    public void ToggleFullscreen()
    {
        if (Screen.fullScreenMode == FullScreenMode.Windowed)
        {
            Screen.fullScreenMode = FullScreenMode.FullScreenWindow;
            isFullscreen = true;
        }
        else
        {
            Screen.fullScreenMode = FullScreenMode.Windowed;
            isFullscreen = false;
        }
        UpdateSprite();
    }

    void UpdateSprite()
    {
        fullscreenButtonImage.sprite = isFullscreen ? fullscreenSprite : windowedSprite;
    }

    public void PlayButton()
    {
        SceneManager.LoadSceneAsync(1);
    }

    public void SetMasterVolume(float volume)
    {
        audioMixer.SetFloat("MasterVolume", Mathf.Log10(Mathf.Max(0.0001f, volume)) * 20);
        volumeTextValue.text = volume.ToString("0.0");

        PlayerPrefs.SetFloat("MasterVolume", volume);
    }



    public void SetMusicVolume(float volume)
    {
        audioMixer.SetFloat("MusicVolume", Mathf.Log10(Mathf.Max(0.0001f, volume)) * 20);
        musicTextValue.text = volume.ToString("0.0");

        PlayerPrefs.SetFloat("MusicVolume", volume);
    }



    public void SetSFXVolume(float volume)
    {
        audioMixer.SetFloat("SFXVolume", Mathf.Log10(Mathf.Max(0.0001f, volume)) * 20);
        sfxTextValue.text = volume.ToString("0.0");

        PlayerPrefs.SetFloat("SFXVolume", volume);
    }


    public void VolumeApply()
    {
        PlayerPrefs.Save();
    }


    public void ResetButton(string MenuType)
    {
        if (MenuType == "Audio")
    {

        audioMixer.SetFloat("MasterVolume", Mathf.Log10(Mathf.Max(0.0001f, defaultVolume)) * 20);
        audioMixer.SetFloat("MusicVolume", Mathf.Log10(Mathf.Max(0.0001f, defaultVolume)) * 20);
        audioMixer.SetFloat("SFXVolume", Mathf.Log10(Mathf.Max(0.0001f, defaultVolume)) * 20);

        masterSlider.value = defaultVolume;
        musicSlider.value = defaultVolume;
        sfxSlider.value = defaultVolume;

        volumeTextValue.text = defaultVolume.ToString("0.0");
        musicTextValue.text = defaultVolume.ToString("0.0");
        sfxTextValue.text = defaultVolume.ToString("0.0");

    VolumeApply();
    }
        if (Screen.fullScreenMode == FullScreenMode.Windowed)
    {
        Screen.fullScreenMode = FullScreenMode.FullScreenWindow;
        isFullscreen = true;
    }
    UpdateSprite();
}

public void ExitButton()
    {
        Application.Quit();
    }
}