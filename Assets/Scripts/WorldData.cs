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
    static public PolygonCollider2D playerOneZone;
    static public PolygonCollider2D playerTwoZone;
    static public PolygonCollider2D playerThreeZone;
    static public PolygonCollider2D playerFourZone;

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

        // Zone colliders are appart from eachother as such: ❖  
        // Player1 is left, Player2 top, Player3 right, Player4 bottom
        playerOneZone = GameObject.Find("PlayerOneZone").GetComponent<PolygonCollider2D>();   // left
        playerTwoZone = GameObject.Find("PlayerTwoZone").GetComponent<PolygonCollider2D>(); ;   // top
        playerThreeZone = GameObject.Find("PlayerThreeZone").GetComponent<PolygonCollider2D>(); ; // right
        playerFourZone = GameObject.Find("PlayerFourZone").GetComponent<PolygonCollider2D>(); ;  // bottom


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
