using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class PlayerTouch : NetworkBehaviour
{
    public GameObject inZone;

    // variable set to true when player is in their territory. variable is set to true in the
    // PlayerZone2 script attached to area where the player's home zone is
    public bool inHomeZone = true;

    private void OnCollisionStay2D(Collision2D collision)
    {
        //if (!hasAuthority || Time.timeSinceLevelLoad < 1.0f)
        if (!hasAuthority)
            return;

        if (inHomeZone)
        {
            if (collision.transform.tag == "PlayerOne")
            {
                if (!collision.gameObject.GetComponent<PlayerTouch>().inHomeZone)
                {
                    if (isServer)
                        RpcSetLocation(collision.gameObject, WorldData2.playerOneSpawnerLocation);
                    else
                        CmdSetLocation(collision.gameObject, WorldData2.playerOneSpawnerLocation);
                }
            }
            if (collision.transform.tag == "PlayerTwo")
            {
                if (!collision.gameObject.GetComponent<PlayerTouch>().inHomeZone)
                {
                    if (isServer)
                        RpcSetLocation(collision.gameObject, WorldData2.playerTwoSpawnerLocation);
                    else
                        CmdSetLocation(collision.gameObject, WorldData2.playerTwoSpawnerLocation);
                }
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

    [ClientRpc]
    private void RpcOnCollidePlayer(NetworkIdentity c)
    {
        if (inZone == null)
            return;

        if (inZone != GetComponent<PlayerAuthority>().ownZone)
            return;

        c.gameObject.transform.position = c.GetComponent<PlayerAuthority>().ownZone.GetComponentInChildren<NetworkStartPosition>().transform.position;
    }

    [Command]
    private void CmdSetLocation(GameObject g, Vector2 l)
    {
        RpcSetLocation(g, l);
    }

    [ClientRpc]
    private void RpcSetLocation(GameObject g, Vector2 l)
    {
        g.transform.position = l; 
    }
}
