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

    void Awake()
    {
        diggableLayer = whatIsDiggableLayer;
        plantableLayer = whatIsPlantableLayer;

    }
}
