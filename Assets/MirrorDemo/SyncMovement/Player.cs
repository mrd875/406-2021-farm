using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class Player : NetworkBehaviour
{
    [SerializeField] private Vector2 movement = new Vector2();

    // client can only execute this
    [Client]
    private void Update()
    {
        // check if we own this object
        if (!hasAuthority) return;

        // check if we pressed the button
        if (!Input.GetKeyDown(KeyCode.Space)) return;


        // do movement (teleport on local client), networktranslate will interpolate the movement to other clients
        //transform.Translate(movement);

        // instead of above, lets tell the server that we want to do a 'move' command on this object.
        CmdMove();
    }


    // a server command, clients can call this function and logic is done on the server
    [Command]
    private void CmdMove()
    {
        // validate movement logic

        // tell everyone that this object moved
        RpcMove();
    }

    // a client rpc, server can tell clients to execute this function
    [ClientRpc]
    private void RpcMove()
    {
        transform.Translate(movement);
    }
}
