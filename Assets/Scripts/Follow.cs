using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Follow : MonoBehaviour
{
    void Start()
    {
        gameObject.transform.position = PlayerData.player.transform.position;
    }

    void Update()
    {
        gameObject.transform.position = PlayerData.player.transform.position;
    }
}
