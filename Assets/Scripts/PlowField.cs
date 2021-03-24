using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using Mirror;

// Must be on object that has authority
public class PlowField : NetworkBehaviour
{
    // Start is called before the first frame update


    public void Activate()
    {
        if (isServer)
            RpcDigAllTiles(PlayerData2.localPlayer.tag);
        else
            CmdDigAllTiles(PlayerData2.localPlayer.tag);
    }

    [Command]
    private void CmdDigAllTiles(string userTag)
    {
        RpcDigAllTiles(userTag);
    }

    [ClientRpc]
    private void RpcDigAllTiles(string userTag)
    {
        Tilemap diggableLayer = new Tilemap();
        if (userTag == "PlayerOne")
            diggableLayer = WorldData2.p1DiggableLayer;
        else if (userTag == "PlayerTwo")
            diggableLayer = WorldData2.p2DiggableLayer;
/*        else
            diggableLayer = WorldData2.p1DiggableLayer;*/

        BoundsInt area = diggableLayer.cellBounds;
        TileBase[] allTiles = diggableLayer.GetTilesBlock(area);
        for (int i = 0; i < allTiles.Length; i++)
        {
            allTiles[i] = null;
        }
        diggableLayer.SetTilesBlock(area, allTiles);
    }
}
