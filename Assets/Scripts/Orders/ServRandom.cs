using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class ServRandom : NetworkBehaviour
{
    // Create a random number to be used as the game seed, synced on the server
    [SyncVar(hook = nameof(RandChanged))]
    public int rand = -1;

    public void RandChanged(int oldIndex, int newIndex) { }

    public override void OnStartServer()
    {
        rand = Random.Range(0, 1000);
    }
}
