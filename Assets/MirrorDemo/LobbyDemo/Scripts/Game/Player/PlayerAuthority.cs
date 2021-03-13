using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class PlayerAuthority : NetworkBehaviour
{
    public GameObject ownZone;

    public override void OnStartAuthority()
    {
        GameObject.FindGameObjectWithTag("GameSpawner").GetComponent<GameSpawner>().localPlayer = gameObject;
        name = "LocalPlayer";
    }

    private void Start()
    {
        var zones = GameObject.FindGameObjectsWithTag("Zone");
        foreach (var zone in zones)
        {
            var spawn = zone.GetComponentInChildren<NetworkStartPosition>();

            if (Vector3.Distance(transform.position, spawn.gameObject.transform.position) < 0.05f)
                ownZone = spawn.transform.parent.gameObject;
        }
    }
}
