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
    static public Shoot2 playerShoot;

    static public float localGrowSpeed = 1;

    private bool setUp = true;

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
                playerOne = GameObject.FindWithTag("PlayerOne");
                playerTwo = GameObject.FindWithTag("PlayerTwo");
                localPlayer = playerAuthority.gameObject;
                playerClick = localPlayer.GetComponent<PlayerClick>();
                playerShoot = localPlayer.GetComponent<Shoot2>();
            }
        }
        if (playerOne == null || playerTwo == null)
            setUp = false;
        //localPlayer = GameObject.Find("LocalPlayer");
    }

    void Update()
    {
        // For if player prefab takes longer than usual to spawn
        if (!setUp)
        {
            StartCoroutine(LateStart(0.1f));
        }
    }
}
