using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Holds data to be passed between scenes
public class ScoreManager : MonoBehaviour
{
    public int score = 10;

    public static ScoreManager instance = null;

    private void Start()
    {
        // Checks for duplicate instances of ScoreManager and clears them
        // Otherwise stops ScoreManager from being destroyed between scenes
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
            Destroy(gameObject);
    }
}
