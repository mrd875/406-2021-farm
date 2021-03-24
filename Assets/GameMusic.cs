using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameMusic : MonoBehaviour
{
    public string gameSceneName;

    // There can be only one
    void Start()
    {
        MenuMusic[] otherMusic = GameObject.FindObjectsOfType<MenuMusic>();
        if (otherMusic.Length != 1)
        {
            //Destroy(this.gameObject);
        }
        //DontDestroyOnLoad(this.gameObject);

        GetComponent<AudioSource>().volume = SoundControl.musicVolume * 0.1f;



    }

    void Update()
    {
        GetComponent<AudioSource>().volume = SoundControl.musicVolume * 0.1f;
        if (SceneManager.GetActiveScene().name != gameSceneName)
        {
            //Make sure game music goes away when game ends
            //Destroy(this.gameObject);
        }
    }
}
