using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class PlayerScore : NetworkBehaviour
{
    [SyncVar]
    public int score = 0;

    public void UpdateScore() {
        Cmds();
    }

    [Command]
    private void Cmds() {
        Rpcs();
    }

    [ClientRpc]
    private void Rpcs() {
        score++;
    }
}
