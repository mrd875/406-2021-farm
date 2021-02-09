using System.Collections;
using System.Collections.Generic;
using UnityEngine.Tilemaps;
using UnityEngine;

public class WorldData : MonoBehaviour
{
    // Interactable tilemaps
    static public Tilemap diggableLayer;
    static public Tilemap plantableLayer;
    static public Tilemap highlighter;

    // Areas on the map belonging to each player
    static public PolygonCollider2D playerOneBoundary;
    static public PolygonCollider2D playerTwoBoundary;
    static public PolygonCollider2D playerThreeBoundary;
    static public PolygonCollider2D playerFourBoundary;

    // Locations where each player spawns after being caught in another's field
    static public Vector2 playerOneSpawnLocation;
    static public Vector2 playerTwoSpawnLocation;
    static public Vector2 playerThreeSpawnLocation;
    static public Vector2 playerFourSpawnLocation;

    static public List<Vector3Int> plantedLocations;

    void Awake()
    {
        diggableLayer = GameObject.Find("DiggableTiles").GetComponent<Tilemap>();
        plantableLayer = GameObject.Find("PlantableTiles").GetComponent<Tilemap>();
        highlighter = GameObject.Find("Highlighter").GetComponent<Tilemap>();
        playerOneSpawnLocation = GameObject.Find("PlayerOneSpawn").transform.position;
        playerTwoSpawnLocation = GameObject.Find("PlayerTwoSpawn").transform.position;

        // Boundary colliders are in order: top to bottom, left to right. 
        // So 0 is top, 1 is left, 2 is right, 3 is bottom: ❖
        PolygonCollider2D[] boundries = GameObject.Find("Boundries").GetComponents<PolygonCollider2D>();
        playerOneBoundary = boundries[1];   // left
        playerTwoBoundary = boundries[0];   // top
        playerThreeBoundary = boundries[2]; // right
        playerFourBoundary = boundries[3];  // bottom


        //Used to tell if a plant has been planted at location before, so seed isn't consumed
        plantedLocations = new List<Vector3Int>();

    }


    static public void AddPlantedLocation(Vector3Int location)
    {
        plantedLocations.Add(location);
    }

    static public void RemovePlantedLocation(Vector3Int location)
    {
        if (plantedLocations.Contains(location))
        {
            plantedLocations.RemoveAt(plantedLocations.IndexOf(location));
        }
        
    }

    static public bool CheckPlantedLocation(Vector3Int location)
    {
        if (plantedLocations.Contains(location))
        {
            return false;
        }
        else
        {
            return true;
        }
    }
}
