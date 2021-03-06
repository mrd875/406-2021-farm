﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class SettingManager : MonoBehaviour
{
    public Dropdown displayMode;
    public Dropdown resolutionDropdown;
    //public Dropdown textureQualityDropdown;
    //public Dropdown antialiasingDropdown;
    //public Dropdown vSyncDropdown;
    public Slider musicVolumeSlider;
    public Slider soundEffectsVolumeSlider;

    public AudioSource musicSource;
    public AudioSource soundEffectsSource;

    public Resolution[] resolutions;
    public GameSettings gameSettings;

    public Button applyButton;
    public Button menuButton;

    public static string SettingPath;

    void Awake()
    {
        SettingPath = Application.persistentDataPath + "/gamesettings.json";
    }

    void Start()
    {
        gameSettings = LoadSettings();
        

        displayMode.onValueChanged.AddListener(delegate { OnDisplayModeChange(); });
        resolutionDropdown.onValueChanged.AddListener(delegate { OnResolutionChange(); });
        //textureQualityDropdown.onValueChanged.AddListener(delegate { OnTextureQualityChange(); });
        //antialiasingDropdown.onValueChanged.AddListener(delegate { OnAntialiasingChange(); });
        //vSyncDropdown.onValueChanged.AddListener(delegate { OnVSyncChange(); });
        musicVolumeSlider.onValueChanged.AddListener(delegate { OnMusicVolumeChange(); });
        soundEffectsVolumeSlider.onValueChanged.AddListener(delegate { OnSoundEffectsVolumeChange(); });

        applyButton.onClick.AddListener(delegate { OnApplyButtonClick(); });
        menuButton.onClick.AddListener(delegate { OnMenuButtonClick(); });


        resolutions = Screen.resolutions;
        foreach(Resolution resolution in resolutions)
        {
            resolutionDropdown.options.Add(new Dropdown.OptionData(resolution.ToString()));
        }

        setUiComponent(gameSettings);

    }

    public void OnDisplayModeChange()
    {
        if (displayMode.value == 0)
        {
            gameSettings.displayMode = displayMode.value;
            Screen.fullScreenMode = FullScreenMode.Windowed;
        }
        else if (displayMode.value == 1)
        {
            gameSettings.displayMode = displayMode.value;
            Screen.fullScreenMode = FullScreenMode.ExclusiveFullScreen;
        }
    }

    public void OnResolutionChange()
    {
        gameSettings.resolutionIndex = resolutionDropdown.value;
        Screen.SetResolution(resolutions[resolutionDropdown.value].width, resolutions[resolutionDropdown.value].height, Screen.fullScreenMode);
    }
    /*
    public void OnTextureQualityChange()
    {
        gameSettings.textureQuality = textureQualityDropdown.value;
        if (textureQualityDropdown.value == 0)
        {
            QualitySettings.masterTextureLimit = 4;
        }
        else if (textureQualityDropdown.value == 1)
        {
            QualitySettings.masterTextureLimit = 3;
        }
        else if (textureQualityDropdown.value == 2)
        {
            QualitySettings.masterTextureLimit = 2;
        }
        else if (textureQualityDropdown.value == 3)
        {
            QualitySettings.masterTextureLimit = 1;
        }
    }

    public void OnAntialiasingChange()
    {
        gameSettings.anitaliasing = QualitySettings.antiAliasing = (int) Mathf.Pow(2, antialiasingDropdown.value);
    }

    public void OnVSyncChange()
    {
        gameSettings.vSync = QualitySettings.vSyncCount = vSyncDropdown.value;
    }
    */
    public void OnMusicVolumeChange()
    {
        SoundControl.musicVolume = musicVolumeSlider.value;
        gameSettings.musicVolume = musicVolumeSlider.value;
    }

    public void OnSoundEffectsVolumeChange()
    {
        SoundControl.soundEffectVolume = soundEffectsVolumeSlider.value;
        gameSettings.soundEffectsVolume = soundEffectsVolumeSlider.value;
    }

    public void OnApplyButtonClick()
    {
        SaveSettings();
    }

    public void OnMenuButtonClick()
    {
        ResetSettings();
    }


    public void ResetSettings()
    {
        gameSettings = LoadSettings();
        if (gameSettings.displayMode == 0)
        {
            Screen.fullScreenMode = FullScreenMode.Windowed;
        }
        else
        {
            Screen.fullScreenMode = FullScreenMode.ExclusiveFullScreen;
        }
        Screen.SetResolution(resolutions[gameSettings.resolutionIndex].width, resolutions[gameSettings.resolutionIndex].height, Screen.fullScreenMode);

        /*
        if (gameSettings.textureQuality == 0)
        {
            QualitySettings.masterTextureLimit = 4;
        }
        else if (gameSettings.textureQuality == 1)
        {
            QualitySettings.masterTextureLimit = 3;
        }
        else if (gameSettings.textureQuality == 2)
        {
            QualitySettings.masterTextureLimit = 2;
        }
        else if (gameSettings.textureQuality == 3)
        {
            QualitySettings.masterTextureLimit = 1;
        }

        QualitySettings.antiAliasing = gameSettings.anitaliasing;
        QualitySettings.vSyncCount = gameSettings.vSync;
        */
        SoundControl.musicVolume = gameSettings.soundEffectsVolume;
        musicSource.volume = gameSettings.musicVolume;

        SoundControl.soundEffectVolume = gameSettings.soundEffectsVolume;
        soundEffectsSource.volume = gameSettings.soundEffectsVolume;

    }

    public void SaveSettings()
    {
        string jsonData = JsonUtility.ToJson(gameSettings, true);
        File.WriteAllText(SettingPath, jsonData);
        Debug.Log("Saved Data");
    }

    private void setUiComponent(GameSettings settings)
    {
        displayMode.value = settings.displayMode;
        resolutionDropdown.value = settings.resolutionIndex;
        //antialiasingDropdown.value = settings.anitaliasing;
        //textureQualityDropdown.value = settings.textureQuality;
        //vSyncDropdown.value = settings.vSync;
        musicVolumeSlider.value = settings.musicVolume;
        soundEffectsVolumeSlider.value = settings.soundEffectsVolume;
    }

    public static GameSettings LoadSettings()
    {
        GameSettings settings;
        if (File.Exists(SettingPath))
        {
            settings = JsonUtility.FromJson<GameSettings>(File.ReadAllText(SettingPath));
        }
        else
        {
            // load default settings
            settings = new GameSettings();
            settings.displayMode = 0;
            settings.resolutionIndex = 0;
            //settings.anitaliasing = 0;
            //settings.vSync = 0;
            //settings.resolutionIndex = 0;
            settings.musicVolume = 0;
            settings.soundEffectsVolume = 0;
        }

        return settings;
    }
}
