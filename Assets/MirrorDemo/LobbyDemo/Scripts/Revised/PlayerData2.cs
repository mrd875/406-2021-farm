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

    public Sprite playerTwoSprite;
    public RuntimeAnimatorController playerTwoAnimatorController;

    static public float localGrowSpeed = 1;
    static public float maxMoveSpeed = 200.0f;

    private bool setUp = true;
    private int setUpAttempt = 0;

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
        if ((playerOne == null || playerTwo == null) && setUpAttempt < 5)
        {
            setUp = false;
            setUpAttempt++; // if this fails 5 times, assume only one player will load in (for testing purpose)
        }
        else
        {
            if (setUpAttempt != 5)
            {
                if (playerTwo != null)
                {
                    playerTwo.GetComponent<SpriteRenderer>().sprite = playerTwoSprite;
                    playerTwo.GetComponent<Animator>().runtimeAnimatorController = playerTwoAnimatorController;
                    playerTwo.GetComponent<PlayerMovement2>().enabled = true;
                    playerTwo.GetComponent<PlayerClick>().enabled = true;
                    playerTwo.GetComponent<Shoot2>().enabled = true;
                }
                else
                {
                    Debug.Log("LateStart PlayerTwo is null");
                }
            }

            if (playerOne != null)
            {
                playerOne.GetComponent<PlayerMovement2>().enabled = true;
                playerOne.GetComponent<PlayerClick>().enabled = true;
                playerOne.GetComponent<Shoot2>().enabled = true;
            }
            else
            {
                Debug.Log("LateStart playerOne is null");
            }

            // Disable collider that blocks trap placement. This is only to exist on other players
            localPlayer.transform.GetChild(1).GetComponent<CircleCollider2D>().enabled = false;
        }
    }

    void Update()
    {
        // For if player prefab takes longer than usual to spawn
        if (!setUp)
        {
            Debug.Log("Player data failed to set values, reattempting...");
            setUp = true; // until proven otherwise in the following function (to prevent message spam)
            StartCoroutine(LateStart(0.5f));
        }
    }
}
