using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class PlayerScore : NetworkBehaviour
{
    [SyncVar]
    public int score = 0;

    public void UpdateScore() {
        CmdUpdateScore();
    }

    [Command]
    private void CmdUpdateScore() {
        RpcUpdateScore();
    }

    [ClientRpc]
    private void RpcUpdateScore() {
        score++;
    }
}
