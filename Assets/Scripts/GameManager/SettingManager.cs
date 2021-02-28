using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class SettingManager : MonoBehaviour
{
    public Dropdown displayMode;
    public Dropdown resolutionDropdown;
    public Dropdown textureQualityDropdown;
    public Dropdown antialiasingDropdown;
    public Dropdown vSyncDropdown;
    public Slider musicVolumeSlider;
    public Slider soundEffectsVolumeSlider;

    public AudioSource musicSource;
    public AudioSource soundEffectsSource;

    public Resolution[] resolutions;
    public GameSettings gameSettings;

    public Button applyButton;

    void OnEnable()
    {
        gameSettings = new GameSettings();

        displayMode.onValueChanged.AddListener(delegate { OnDisplayModeChange(); });
        resolutionDropdown.onValueChanged.AddListener(delegate { OnResolutionChange(); });
        textureQualityDropdown.onValueChanged.AddListener(delegate { OnTextureQualityChange(); });
        antialiasingDropdown.onValueChanged.AddListener(delegate { OnAntialiasingChange(); });
        vSyncDropdown.onValueChanged.AddListener(delegate { OnVSyncChange(); });
        musicVolumeSlider.onValueChanged.AddListener(delegate { OnMusicVolumeChange(); });
        soundEffectsVolumeSlider.onValueChanged.AddListener(delegate { OnSoundEffectsVolumeChange(); });

        applyButton.onClick.AddListener(delegate { OnApplyButtonClick(); });


        resolutions = Screen.resolutions;
        foreach(Resolution resolution in resolutions)
        {
            resolutionDropdown.options.Add(new Dropdown.OptionData(resolution.ToString()));
        }

        LoadSettings();
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
            Debug.Log(displayMode.value);
        }
    }

    public void OnResolutionChange()
    {
        gameSettings.resolutionIndex = resolutionDropdown.value;
        Screen.SetResolution(resolutions[resolutionDropdown.value].width, resolutions[resolutionDropdown.value].height, Screen.fullScreenMode);
    }

    public void OnTextureQualityChange()
    {
        if (textureQualityDropdown.value == 0)
        {
            gameSettings.textureQuality = QualitySettings.masterTextureLimit = 4;
        }
        else if (textureQualityDropdown.value == 1)
        {
            gameSettings.textureQuality = QualitySettings.masterTextureLimit = 3;
        }
        else if (textureQualityDropdown.value == 2)
        {
            gameSettings.textureQuality = QualitySettings.masterTextureLimit = 2;
        }
        else if (textureQualityDropdown.value == 3)
        {
            gameSettings.textureQuality = QualitySettings.masterTextureLimit = 1;
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

    public void OnMusicVolumeChange()
    {
        gameSettings.musicVolume = musicSource.volume = musicVolumeSlider.value;
    }

    public void OnSoundEffectsVolumeChange()
    {
        gameSettings.soundEffectsVolume = soundEffectsSource.volume = soundEffectsVolumeSlider.value;
    }

    public void OnApplyButtonClick()
    {
        SaveSettings();
    }

    public void SaveSettings()
    {
        string jsonData = JsonUtility.ToJson(gameSettings, true);
        File.WriteAllText(Application.persistentDataPath + "/gamesettings.json", jsonData);
    }

    public void LoadSettings()
    {
        gameSettings = JsonUtility.FromJson<GameSettings>(File.ReadAllText(Application.persistentDataPath + "/gamesettings.json"));

        displayMode.value = gameSettings.displayMode;
        resolutionDropdown.value = gameSettings.resolutionIndex;
        antialiasingDropdown.value = gameSettings.anitaliasing;
        textureQualityDropdown.value = gameSettings.textureQuality;
        vSyncDropdown.value = gameSettings.vSync;
        musicVolumeSlider.value = gameSettings.musicVolume;
        soundEffectsVolumeSlider.value = gameSettings.soundEffectsVolume;
    }
}
