using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class GameSpawner : MonoBehaviour
{
    private NetworkRoomManagerGame _netman;
    private NetworkRoomManagerGame NetMan
    {
        get
        {
            if (_netman != null) return _netman;

            _netman = NetworkManager.singleton as NetworkRoomManagerGame;
            if (_netman != null) return _netman;

            _netman = FindObjectOfType<NetworkRoomManagerGame>();
            return _netman;
        }
    }

    [HideInInspector]
    public GameObject localPlayer;
}
