using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using Mirror;

public class PlayerClick : NetworkBehaviour
{
    private Tilemap gl;


    private void Start()
    {
        gl = GameObject.FindGameObjectWithTag("GameGrid").GetComponent<Tilemap>();
    }

    private void Update()
    {
        // only listen for clicks on our own game object.
        if (!hasAuthority)
            return;

        if (Input.GetMouseButtonDown(0))
        {
            // get the position of the click

            Vector2 mousePos = Input.mousePosition;
            Vector2 worldPosition2D = Camera.main.ScreenToWorldPoint(mousePos);
            Vector3 worldPosition = new Vector3(worldPosition2D.x, worldPosition2D.y, transform.position.z);
            Vector3Int worldPos = gl.WorldToCell(worldPosition);

            // locally update our tile
            gl.SetTile(worldPos, null);

            // tell the server to tell other clients about our click
            CmdSetTile(worldPos);
        }
    }

    [Command]
    private void CmdSetTile(Vector3Int v)
    {
        // tell other clients about our click
        RpcSetTile(v);
    }

    [ClientRpc]
    private void RpcSetTile(Vector3Int v)
    {
        // update our tile
        gl.SetTile(v, null);
    }
}
