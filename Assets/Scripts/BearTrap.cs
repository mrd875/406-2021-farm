﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using UnityEngine.EventSystems;

public class BearTrap : NetworkBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public int id;
    public string trapOwnerTag;
    public float trapTime = 2.5f;
    public float visibilityDistance = 2.0f;
    private bool triggered = false;

    public HealthBar durabilityBar;
    public int maxDurability = 10;

    public int durability;

    public void OnPointerEnter(PointerEventData eventData)
    {
        PlayerData2.playerClick.highlightedInteractable = gameObject;
        if (PlayerData2.playerClick.canInteract)
            gameObject.transform.GetChild(1).gameObject.SetActive(true);
    }


    public void OnPointerExit(PointerEventData eventData)
    {
        PlayerData2.playerClick.highlightedInteractable.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1);
        PlayerData2.playerClick.highlightedInteractable = null;
        gameObject.transform.GetChild(1).gameObject.SetActive(false);
    }

    void Start()
    {
        StartCoroutine(LateStart(0.2f));
        durabilityBar.SetMaxHealth(maxDurability);
        durability = maxDurability;
        if (trapOwnerTag == PlayerData2.localPlayer.tag)
            GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1);
        else
            GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 0);
        GetComponent<Animator>().enabled = false;
    }

    IEnumerator LateStart(float startTime)
    {
        yield return new WaitForSeconds(startTime);
        if (PlayerData2.localPlayer.tag == trapOwnerTag)
            gameObject.layer = 0;
    }

    void Update()
    {
        if (trapOwnerTag != PlayerData2.localPlayer.tag && triggered == false)
        {
            if (Vector2.Distance(PlayerData2.localPlayer.transform.position, transform.position) < visibilityDistance )
                GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 0.5f);
            else
            {
                GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 0);
            }
        }

        if (durability <= 0)
        {
            Destroy(this.gameObject);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag != PlayerData2.localPlayer.tag)
            return;

        if (triggered)
            return;
        if (other.tag == "PlayerOne" || other.tag == "PlayerTwo")
        {
            if (other.tag != trapOwnerTag)
            {
                if (isServer)
                    RpcTriggered(PlayerData2.localPlayer);
                else
                    CmdTriggered(PlayerData2.localPlayer);

                /*triggered = true;
                other.transform.position = gameObject.transform.position + new Vector3(0, 0.7f, 0);
                PlayerMovement2 playerMovement = other.GetComponent<PlayerMovement2>();
                StartCoroutine(playerMovement.Trapped(this.gameObject, trapTime));
                playerMovement.gameObject.GetComponent<SpriteRenderer>().color = Color.red;
                SoundControl.PlayTrapSound();
                GetComponent<Animator>().enabled = true;
                GetComponent<SpriteRenderer>().sortingOrder = 1;
                GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1);*/
            }
        }
    }

    public void Damage()
    {
        durability -= 1;
        durabilityBar.SetHealth(durability);
    }

    [Command(ignoreAuthority = true)]
    void CmdTriggered(GameObject player)
    {
        RpcTriggered(player);
    }

    [ClientRpc]
    void RpcTriggered(GameObject player)
    {
        triggered = true;
        player.transform.position = gameObject.transform.position + new Vector3(0, 0.7f, 0);
        PlayerMovement2 playerMovement = player.GetComponent<PlayerMovement2>();
        StartCoroutine(playerMovement.Trapped(this.gameObject, trapTime));
        playerMovement.gameObject.GetComponent<SpriteRenderer>().color = Color.red;
        SoundControl.PlayTrapSound();
        GetComponent<Animator>().enabled = true;
        GetComponent<SpriteRenderer>().sortingOrder = 1;
        GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1);
    }
}
