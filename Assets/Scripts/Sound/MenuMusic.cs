using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuMusic : MonoBehaviour
{

    public string gameSceneName;

    // There can be only one
    void Start()
    {

        SoundControl.Load();

        MenuMusic[] otherMusic = GameObject.FindObjectsOfType<MenuMusic>();
        if (otherMusic.Length != 1)
        {
            Destroy(this.gameObject);
        }
        DontDestroyOnLoad(this.gameObject);

        GetComponent<AudioSource>().volume = SoundControl.musicVolume * 0.5f;
        

    }

    void Update()
    {
        GetComponent<AudioSource>().volume = SoundControl.musicVolume * 0.5f;
        if (SceneManager.GetActiveScene().name == gameSceneName)
        {
            //Make sure main menu music goes away when game starts
            Destroy(this.gameObject);
        }
    }

}
