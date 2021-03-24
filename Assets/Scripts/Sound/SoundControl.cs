using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;


public class SoundControl : MonoBehaviour
{
    // Start is called before the first frame update
    public static float soundEffectVolume = 0.5f;
    public static float musicVolume = 0.5f;

    public static void Load()
    {
        GameSettings gameSettings = JsonUtility.FromJson<GameSettings>(File.ReadAllText(Application.persistentDataPath + "/gamesettings.json"));

        musicVolume = gameSettings.musicVolume;
        soundEffectVolume = gameSettings.soundEffectsVolume;
    }
    public static void PlayButtonSound()
    {
        Debug.Log("SFX volume: " + soundEffectVolume.ToString());
        ButtonSound soundSource = GameObject.FindObjectOfType<ButtonSound>();
        soundSource.GetComponent<AudioSource>().volume = soundEffectVolume;
        soundSource.GetComponent<AudioSource>().Play();
    }

    public static void PlayShovelSound()
    {
        ShovelSound soundSource = GameObject.FindObjectOfType<ShovelSound>();
        soundSource.GetComponent<AudioSource>().volume = soundEffectVolume;
        soundSource.GetComponent<AudioSource>().Play();
    }

    public static void PlayOrderSound()
    {
        OrderSound soundSource = GameObject.FindObjectOfType<OrderSound>();
        soundSource.GetComponent<AudioSource>().volume = soundEffectVolume * 0.1f;
        soundSource.GetComponent<AudioSource>().Play();
    }
    public static void PlayPurchaseSound()
    {
        PurchaseSound soundSource = GameObject.FindObjectOfType<PurchaseSound>();
        soundSource.GetComponent<AudioSource>().volume = soundEffectVolume;
        soundSource.GetComponent<AudioSource>().Play();
    }

    public static void PlayMoneySound()
    {
        MoneySound soundSource = GameObject.FindObjectOfType<MoneySound>();
        soundSource.GetComponent<AudioSource>().volume = soundEffectVolume;
        soundSource.GetComponent<AudioSource>().Play();
    }
    public static void PlayShopSound()
    {
        ShopSound soundSource = GameObject.FindObjectOfType<ShopSound>();
        soundSource.GetComponent<AudioSource>().volume = soundEffectVolume;
        soundSource.GetComponent<AudioSource>().Play();
    }

    public static void PlayPlantSound()
    {
        PlantSound soundSource = GameObject.FindObjectOfType<PlantSound>();
        soundSource.GetComponent<AudioSource>().volume = soundEffectVolume;
        soundSource.GetComponent<AudioSource>().Play();
    }

}
