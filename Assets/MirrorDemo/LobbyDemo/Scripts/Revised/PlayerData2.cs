using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class PlayerData2 : MonoBehaviour
{
    static public GameObject playerOne;
    static public GameObject playerTwo;
    static public GameObject localPlayer;

    // Start is called before the first frame update
    void Start()
    {
        // Wait for network stuff to be ready
        StartCoroutine(LateStart(0.05f));
    }

    IEnumerator LateStart(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        localPlayer = GameObject.Find("LocalPlayer");
    }
}
