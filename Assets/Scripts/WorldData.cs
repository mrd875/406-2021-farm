using System.Collections;
using System.Collections.Generic;
using UnityEngine.Tilemaps;
using UnityEngine;

public class WorldData : MonoBehaviour
{
    public Tilemap whatIsTopLayer;
    public Tilemap whatIsBaseLayer;
    public Tile whatIsDirt;
    public Tile whatIsGrass;

    static public Tilemap topLayer;
    static public Tilemap baseLayer;
    static public Tile dirt;
    static public Tile grass;

    void Awake()
    {
        topLayer = whatIsTopLayer;
        baseLayer = whatIsBaseLayer;
        dirt = whatIsDirt;
        grass = whatIsGrass;
    }
}
