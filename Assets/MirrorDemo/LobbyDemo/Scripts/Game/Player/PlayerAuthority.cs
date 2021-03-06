﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class PlayerAuthority : NetworkBehaviour
{
    public GameObject ownZone;

    public override void OnStartAuthority()
    {
        GameObject.FindGameObjectWithTag("GameSpawner").GetComponent<GameSpawner>().localPlayer = gameObject;
        name = "LocalPlayer";
    }

    private void Start()
    {
        if (gameObject.transform.position.y > 8)
        {
            gameObject.tag = "PlayerTwo";
        }
        else
        {
            gameObject.tag = "PlayerOne";
        }

        var zones = GameObject.FindGameObjectsWithTag("Zone");
        foreach (var zone in zones)
        {
            var spawn = zone.GetComponentInChildren<NetworkStartPosition>();

            if (Vector3.Distance(transform.position, spawn.gameObject.transform.position) < 0.05f)
                ownZone = spawn.transform.parent.gameObject;
        }
    }

    public void ShutDownSetup()
    {
        CmdShutdownSetup();
        //ShutDownServer();
    }

    [Command]
    private void CmdShutdownSetup()
    {
        RpcShutdownSetup();
        PersistentRoundInfo[] cleanUp = FindObjectsOfType<PersistentRoundInfo>();
        foreach (var persistentRoundInfo in cleanUp)
        {
            Destroy(persistentRoundInfo.gameObject);
        }
        PlayerData.money = 100;
        PlayerData2.localGrowSpeed = 1.0f;
        PlayerMovement2 playerMoveScript = PlayerData2.localPlayer.GetComponent<PlayerMovement2>();
        playerMoveScript.ResetSpeed();

    }

    [ClientRpc]
    private void RpcShutdownSetup()
    {
        PersistentRoundInfo[] cleanUp = FindObjectsOfType<PersistentRoundInfo>();
        foreach (var persistentRoundInfo in cleanUp)
        {
            Destroy(persistentRoundInfo.gameObject);
        }
        PlayerData.money = 100;
        PlayerData2.localGrowSpeed = 1.0f;
        PlayerMovement2 playerMoveScript = PlayerData2.localPlayer.GetComponent<PlayerMovement2>();
        playerMoveScript.ResetSpeed();
    }
    /*
    [Server]
    void ShutDownServer()
    {
        NetworkManager thisManager = GameObject.FindObjectOfType<NetworkManager>();
        thisManager.ServerChangeScene("Scene_Lobby");

        NetworkServer.Shutdown();
    }
    */

}
