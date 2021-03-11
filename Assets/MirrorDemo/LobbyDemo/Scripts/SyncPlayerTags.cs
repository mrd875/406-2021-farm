using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;


// Sets tags of all instances of the game to be the same
public class SyncPlayerTags : NetworkBehaviour
{
    void Start()
    {
        // Initially, the client player objects are the only ones with set tags (player one is not set here for some reason so added part to update) 
        if (gameObject.tag != "Player")
        {
            RpcSyncTags(gameObject.tag);
        }
    }

    public void SyncUp()
    {
        // Initially, the client player objects are the only ones with set tags (player one is not set here for some reason so added part to update) 
        if (gameObject.tag != "Player")
        {
            RpcSyncTags(gameObject.tag);
        }
    }
    void Update()
    {
        if (gameObject.tag == "Player")
        {
            gameObject.tag = "PlayerOne";
        }
    }

    [Command]
    private void CmdSyncTags(string tag)
    {
        gameObject.tag = tag;
        RpcSyncTags(tag);
    }

    [ClientRpc]
    private void RpcSyncTags(string tag)
    {
        gameObject.tag = tag;
    }
}
