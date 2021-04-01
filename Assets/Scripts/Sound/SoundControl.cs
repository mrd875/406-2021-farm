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
        var gameSettings = SettingManager.LoadSettings();

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
        ShovelSound[] soundSource = GameObject.FindObjectsOfType<ShovelSound>();

        int roll = Mathf.RoundToInt(Random.Range(0, 2));

        soundSource[roll].GetComponent<AudioSource>().volume = soundEffectVolume;
        soundSource[roll].GetComponent<AudioSource>().Play();
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

    public static void PlayMoveSound()
    {
        MoveSound soundSource = GameObject.FindObjectOfType<MoveSound>();
        soundSource.GetComponent<AudioSource>().volume = soundEffectVolume;
        soundSource.GetComponent<AudioSource>().Play();
    }


    public static void StopMoveSound()
    {
        MoveSound soundSource = GameObject.FindObjectOfType<MoveSound>();
        soundSource.GetComponent<AudioSource>().Stop();
    }

    public static void PlayHarvestSound()
    {
        HarvestSound soundSource = GameObject.FindObjectOfType<HarvestSound>();
        soundSource.GetComponent<AudioSource>().volume = soundEffectVolume;
        soundSource.GetComponent<AudioSource>().Play();
    }

    public static void PlayBadSound()
    {
        BadSound soundSource = GameObject.FindObjectOfType<BadSound>();
        soundSource.GetComponent<AudioSource>().volume = soundEffectVolume * 0.1f;
        soundSource.GetComponent<AudioSource>().Play();
    }

    public static void PlayWinSound()
    {
        WinSound soundSource = GameObject.FindObjectOfType<WinSound>();
        soundSource.GetComponent<AudioSource>().volume = soundEffectVolume * 0.2f;
        soundSource.GetComponent<AudioSource>().Play();
    }

    public static void PlayWaterSound()
    {
        WaterSound soundSource = GameObject.FindObjectOfType<WaterSound>();
        soundSource.GetComponent<AudioSource>().volume = soundEffectVolume * 0.5f;
        soundSource.GetComponent<AudioSource>().Play();
    }
    public static void PlayTrapSound()
    {
        TrapSound soundSource = GameObject.FindObjectOfType<TrapSound>();
        soundSource.GetComponent<AudioSource>().volume = soundEffectVolume * 0.2f;
        soundSource.GetComponent<AudioSource>().Play();
    }
    public static void PlayRechargeSound()
    {
        RechargeSound soundSource = GameObject.FindObjectOfType<RechargeSound>();
        soundSource.GetComponent<AudioSource>().volume = soundEffectVolume * 0.3f;
        soundSource.GetComponent<AudioSource>().Play();
    }
    public static void PlayStartSound()
    {
        StartSound soundSource = GameObject.FindObjectOfType<StartSound>();
        soundSource.GetComponent<AudioSource>().volume = soundEffectVolume * 0.3f;
        soundSource.GetComponent<AudioSource>().Play();
    }
}
