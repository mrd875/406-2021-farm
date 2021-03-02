using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class PlayerTouch : NetworkBehaviour
{
    public GameObject inZone;

    // variable set to true when player is in their territory. variable is set to true in the
    // PlayerZone2 script attached to area where the player's home zone is
    public bool inHomeZone = false;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!hasAuthority || Time.timeSinceLevelLoad < 1.0f)
            return;

        if (inHomeZone)
        {
            if (collision.transform.tag == "PlayerOne")
            {
                if (isServer)
                    RpcSetLocation(collision.gameObject, WorldData.playerOneSpawnLocation);
                else
                    CmdSetLocation(collision.gameObject, WorldData.playerOneSpawnLocation);
            }
            if (collision.transform.tag == "PlayerTwo")
            {
                if (isServer)
                    RpcSetLocation(collision.gameObject, WorldData.playerTwoSpawnLocation);
                else
                    CmdSetLocation(collision.gameObject, WorldData.playerTwoSpawnLocation);
            }
        }

/*        if (collision.gameObject.tag == "Player")
        {
            CmdOnCollidePlayer(collision.gameObject.GetComponent<NetworkIdentity>());
        }*/
    }

    [Command]
    private void CmdOnCollidePlayer(NetworkIdentity c)
    {
        RpcOnCollidePlayer(c);
    }

    [Command]
    private void CmdSetLocation(GameObject g, Vector2 l)
    {
        RpcSetLocation(g, l);
    }

    [ClientRpc]
    private void RpcOnCollidePlayer(NetworkIdentity c)
    {
        if (inZone == null)
            return;

        if (inZone != GetComponent<PlayerAuthority>().ownZone)
            return;

        c.gameObject.transform.position = c.GetComponent<PlayerAuthority>().ownZone.GetComponentInChildren<NetworkStartPosition>().transform.position;
    }

    [ClientRpc]
    private void RpcSetLocation(GameObject g, Vector2 l)
    {
        g.transform.position = l; 
    }
}
