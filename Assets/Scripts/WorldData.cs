using System.Collections;
using System.Collections.Generic;
using UnityEngine.Tilemaps;
using UnityEngine;

public class WorldData : MonoBehaviour
{
    static public Tilemap diggableLayer;
    static public Tilemap plantableLayer;

    static public Vector2 playerOneSpawnLocation;
    static public Vector2 playerTwoSpawnLocation;

    void Awake()
    {
        diggableLayer = GameObject.Find("DiggableTiles").GetComponent<Tilemap>();
        plantableLayer = GameObject.Find("PlantableTiles").GetComponent<Tilemap>();
        playerOneSpawnLocation = GameObject.Find("PlayerOneSpawn").transform.position;
        playerTwoSpawnLocation = GameObject.Find("PlayerTwoSpawn").transform.position;
    }
}
