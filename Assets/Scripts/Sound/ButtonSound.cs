using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonSound : MonoBehaviour
{
    // There can be only one
    void Start()
    {
        ButtonSound[] otherMusic = GameObject.FindObjectsOfType<ButtonSound>();
        if (otherMusic.Length != 1)
        {
            Destroy(this.gameObject);
        }
        DontDestroyOnLoad(this);

    }

}
