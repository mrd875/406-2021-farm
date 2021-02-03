using System.Collections;
using System.Collections.Generic;
using UnityEngine.Tilemaps;
using UnityEngine;

public class WorldData : MonoBehaviour
{
    public Tilemap whatIsDiggableLayer;
    public Tilemap whatIsPlantableLayer;

    static public Tilemap diggableLayer;
    static public Tilemap plantableLayer;

    public Transform setPlayerOneSpawn;
    public Transform setPlayerTwoSpawn;

    static public Transform playerOneSpawn;
    static public Transform playerTwoSpawn;

    void Awake()
    {
        diggableLayer = whatIsDiggableLayer;
        plantableLayer = whatIsPlantableLayer;
        playerOneSpawn = setPlayerOneSpawn;
        playerTwoSpawn = setPlayerTwoSpawn;
    }
}
