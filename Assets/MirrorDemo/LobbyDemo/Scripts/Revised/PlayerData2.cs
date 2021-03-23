using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class PlayerData2 : NetworkBehaviour
{
    static public GameObject playerOne;
    static public GameObject playerTwo;
    static public GameObject localPlayer;
    static public PlayerClick playerClick;

    static public float localGrowSpeed = 1;

    // Start is called before the first frame update
    void Start()
    {
        // Wait for network stuff to be ready
        StartCoroutine(LateStart(0.5f));
    }

    IEnumerator LateStart(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        PlayerAuthority[] players = GameObject.FindObjectsOfType<PlayerAuthority>();
        foreach (var playerAuthority in players)
        {
            if (playerAuthority.hasAuthority)
            {
                localPlayer = playerAuthority.gameObject;
                playerClick = localPlayer.GetComponent<PlayerClick>();
            }
        }
        //localPlayer = GameObject.Find("LocalPlayer");
    }
}
