using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class PlayerTouch : NetworkBehaviour
{
    public GameObject inZone;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!hasAuthority || Time.timeSinceLevelLoad < 1.0f)
            return;

        if (collision.gameObject.tag == "Player")
        {
            CmdOnCollidePlayer(collision.gameObject.GetComponent<NetworkIdentity>());
        }
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
}
