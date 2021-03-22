using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using Mirror;

public class PlowField : NetworkBehaviour
{
    // Start is called before the first frame update


    public void Activate()
    {
        if (isServer)
            RpcDigAllTiles();
        else
            CmdDigAllTiles();
    }

    [Command]
    private void CmdDigAllTiles()
    {
        RpcDigAllTiles();
    }

    [ClientRpc]
    private void RpcDigAllTiles()
    {
        Tilemap diggableLayer = new Tilemap();
        if (PlayerData2.localPlayer.tag == "PlayerOne")
            diggableLayer = WorldData2.p1DiggableLayer;
        else if (PlayerData2.localPlayer.tag == "PlayerTwo")
            diggableLayer = WorldData2.p2DiggableLayer;
        else
            diggableLayer = WorldData2.p1DiggableLayer;

        BoundsInt area = diggableLayer.cellBounds;
        TileBase[] allTiles = diggableLayer.GetTilesBlock(area);
        for (int i = 0; i < allTiles.Length; i++)
        {
            allTiles[i] = null;
        }
        diggableLayer.SetTilesBlock(area, allTiles);
    }
}
