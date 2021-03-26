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

    public static string SettingPath;

    void Awake()
    {
        SettingPath = Application.persistentDataPath + "/gamesettings.json";
    }

    void OnEnable()
    {
        gameSettings = LoadSettings();
        setUiComponent(gameSettings);

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
        SoundControl.musicVolume = musicVolumeSlider.value;
        gameSettings.musicVolume = musicSource.volume = musicVolumeSlider.value;
    }

    public void OnSoundEffectsVolumeChange()
    {
        SoundControl.soundEffectVolume = soundEffectsVolumeSlider.value;
        gameSettings.soundEffectsVolume = soundEffectsSource.volume = soundEffectsVolumeSlider.value;
    }

    public void OnApplyButtonClick()
    {
        SaveSettings();
    }

    public void SaveSettings()
    {
        string jsonData = JsonUtility.ToJson(gameSettings, true);
        File.WriteAllText(SettingPath, jsonData);
    }

    private void setUiComponent(GameSettings settings)
    {
        displayMode.value = settings.displayMode;
        resolutionDropdown.value = settings.resolutionIndex;
        antialiasingDropdown.value = settings.anitaliasing;
        textureQualityDropdown.value = settings.textureQuality;
        vSyncDropdown.value = settings.vSync;
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
            settings.anitaliasing = 0;
            settings.vSync = 0;
            settings.resolutionIndex = 0;
            settings.musicVolume = 0;
            settings.soundEffectsVolume = 0;
        }

        return settings;
    }
}
