using System.Collections;
using System.Collections.Generic;
using UnityEngine.Tilemaps;
using UnityEngine;

public class WorldData2 : MonoBehaviour
{
    // Interactable tilemaps
    static public Tilemap diggableLayer;
    static public Tilemap plantableLayer;
    static public Tilemap highlighter;

    public struct plantInfo
    {
        public GameObject plantObject;
        public string plantName;
    }

    public static List<plantInfo> plants;

    // Areas on the map belonging to each player
    static public PolygonCollider2D playerOneZone;
    static public PolygonCollider2D playerTwoZone;


    // Locations where each player spawns after being caught in another's field
/*    static public Vector2 playerOneSpawnLocation;
    static public Vector2 playerTwoSpawnLocation;
    static public Vector2 playerThreeSpawnLocation;
    static public Vector2 playerFourSpawnLocation;*/

    public struct plantWorldInfo
    {
        public GameObject plant;
        public Vector3Int plantLocation;
        public int ID;
    }

    public static int currentID = 0;
    public static List<plantWorldInfo> plantedLocations;


    void Awake()
    {
        diggableLayer = GameObject.Find("DiggableTiles").GetComponent<Tilemap>();
        plantableLayer = GameObject.Find("PlantableTiles").GetComponent<Tilemap>();
        highlighter = GameObject.Find("Highlighter").GetComponent<Tilemap>();

        playerOneZone = GameObject.Find("PlayerOneZone").GetComponent<PolygonCollider2D>();   // left
        playerTwoZone = GameObject.Find("PlayerTwoZone").GetComponent<PolygonCollider2D>(); ;   // right

        //Used to plant
        plantedLocations = new List<plantWorldInfo>();
        plants = new List<plantInfo>();

    }


    //GameObject plant
    public static bool AddPlantedLocation(Vector3Int location, string plantName)
    {
        if (CheckPlantedLocation(location))
        {
            plantInfo plantToPlace = new plantInfo();
            plantWorldInfo worldInfo = new plantWorldInfo();
            bool foundPlant = false;
            worldInfo.plantLocation = location;

            //Find the right plant in the DB
            foreach (plantInfo plant in plants)
            {
                if (plant.plantName.Equals(plantName))
                {
                    plantToPlace = plant;
                    foundPlant = true;
                }
            }

            //If the plant exists in the DB, spawn it
            if (foundPlant)
            {
                worldInfo.plant = Instantiate(plantToPlace.plantObject, WorldData2.plantableLayer.CellToWorld(location), Quaternion.identity);
                worldInfo.plant.GetComponent<GrowVegetable>().ID = currentID;
                worldInfo.ID = currentID;
                currentID += 1;
                plantedLocations.Add(worldInfo);
                Debug.Log("Created plant with ID: " + (currentID - 1).ToString());
                return true;
            }

            return false;
        }
        else
        {
            return false;
        }
       
    }

    //Used by PlantDB to transfer in plant info
    public static void CreateAndAddPlant(string plantName, GameObject plantObj)
    {
        plantInfo newPlant = new plantInfo();
        newPlant.plantName = plantName;
        newPlant.plantObject = plantObj;

        plants.Add(newPlant);

    }

    public static void RemoveItemsWithID(int id)
    {
        int usableID = id / 256;
        Debug.Log("Destroying plant of id: " + usableID.ToString());
        Item2[] allItems = GameObject.FindObjectsOfType<Item2>();
        foreach (var allItem in allItems)
        {
            Debug.Log("Examining: " + allItem.name);
            plantID thisID;
            if ( (thisID = allItem.GetComponent<plantID>()) != null)
            {
                Debug.Log("This one has an ID of: " + thisID.ID.ToString());
                if (thisID.ID == usableID)
                {
                    Debug.Log("Got a hit, destroying");
                    RemoveByID(thisID.ID);
                    Destroy(thisID.gameObject);
                }
            }
        }
    }

    private static void RemoveByID(int ID)
    {
        foreach (var plantWorldInfo in plantedLocations)
        {
            if (plantWorldInfo.ID == ID)
            {
                plantedLocations.RemoveAt(plantedLocations.IndexOf(plantWorldInfo));
                return;
            }
        }
    }
    public static int RemovePlantedLocation(Vector3Int location)
    {
        foreach (var plantWorldInfo in plantedLocations)
        {
            if (plantWorldInfo.plantLocation == location)
            {
                int returnVal = plantWorldInfo.ID;
                Debug.Log("Got a location hit at ID " + returnVal.ToString());
                Destroy(plantWorldInfo.plant.gameObject);
                plantedLocations.RemoveAt(plantedLocations.IndexOf(plantWorldInfo));
                return returnVal;
            }
        }

        return -1;

    }

    public static bool CheckPlantedLocation(Vector3Int location)
    {
        foreach (var plantWorldInfo in plantedLocations)
        {
            if (plantWorldInfo.plantLocation == location)
            {
                return false;
            }
        }

        return true;

    }
}
