using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine.Tilemaps;
using UnityEngine;

public class WorldData2 : MonoBehaviour
{
    // Interactable tilemaps
    static public Tilemap p1DiggableLayer;
    static public Tilemap p2DiggableLayer;
    static public Tilemap plantableLayer;
    static public Tilemap highlighter;

    //Used as a database for spawning in plants
    public struct plantInfo
    {
        public GameObject plantObject;
        public string plantName;
    }
    public static List<plantInfo> plants;

    // Areas on the map belonging to each player
    static public PolygonCollider2D playerOneZone;
    static public PolygonCollider2D playerTwoZone;



    //Used to delete plants across clients. Tracks spawned plant info.
    public struct plantWorldInfo
    {
        public GameObject plant;
        public Vector3Int plantLocation;
        public int ID;
    }
    public static int currentID = 0;
    public static List<plantWorldInfo> plantedLocations;

    // Locations where each player spawns after being caught in another's field
    static public Vector2 playerOneSpawnerLocation;
    static public Vector2 playerTwoSpawnerLocation;
    //static public Vector2 playerThreeSpawnLocation;
    //static public Vector2 playerFourSpawnLocation;


    void Awake()
    {
        p1DiggableLayer = GameObject.Find("P1DiggableTiles").GetComponent<Tilemap>();
        p2DiggableLayer = GameObject.Find("P2DiggableTiles").GetComponent<Tilemap>();
        plantableLayer = GameObject.Find("PlantableTiles").GetComponent<Tilemap>();
        highlighter = GameObject.Find("Highlighter").GetComponent<Tilemap>();


        // Spawn locations for each player
        playerOneSpawnerLocation = GameObject.Find("PlayerOneSpawner").transform.position;
        playerTwoSpawnerLocation = GameObject.Find("PlayerTwoSpawner").transform.position;

        playerOneZone = GameObject.Find("PlayerOneZone").GetComponent<PolygonCollider2D>();   // left
        playerTwoZone = GameObject.Find("PlayerTwoZone").GetComponent<PolygonCollider2D>(); ;   // right

        //Used to plant
        plantedLocations = new List<plantWorldInfo>();
        plants = new List<plantInfo>();

    }


    //Plants a plant at location with name plantName (matching in the PlantDB)
    public static bool AddPlantedLocation(Vector3Int location, string plantName, float growthRate)
    {
        if (CheckPlantedLocation(location))
        {
            //Can plant. Find which plant to place, and create a plantWorldInfo for it
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

            //If the plant exists in the DB, spawn it, create and add a plantWorldInfo record for it
            if (foundPlant)
            {
                worldInfo.plant = Instantiate(plantToPlace.plantObject, WorldData2.plantableLayer.GetCellCenterWorld(location), Quaternion.identity);
                worldInfo.plant.GetComponent<GrowVegetable>().ID = currentID;
                worldInfo.plant.GetComponent<GrowVegetable>().StartGrowing(growthRate);
                worldInfo.ID = currentID;
                currentID += 1;
                plantedLocations.Add(worldInfo);
                Debug.Log("Created plant with ID: " + (currentID - 1).ToString());
                return true;
            }

            //Couldn't find plant
            Debug.Log("here");
            return false;
        }
        else
        {
            //Couldn't place plant (Location already in use)
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




    //Removes the plant with the given unique ID. Used by other clients to remove plants (location is unreliable)
    public static void RemoveItemsWithID(int id)
    {
        //For some reason IDs get multiplied by 256. Extract usable ID
        int usableID = id / 256;
        Debug.Log("Destroying plant of id: " + usableID.ToString());

        GrowVegetable[] growingVegetables = GameObject.FindObjectsOfType<GrowVegetable>();
        Item2[] allItems = GameObject.FindObjectsOfType<Item2>();
        
        //For all growing vegetables...
        foreach (var growingVegetable in growingVegetables)
        {
            if (growingVegetable.ID == usableID)
            {
                Debug.Log("Got a hit, destroying");
                RemoveByID(growingVegetable.ID);
                growingVegetable.transform.position = new Vector3(-500, 0, 0);
            }
        }

        //For all items...
        foreach (var allItem in allItems)
        {
            plantID thisID;
            if ( (thisID = allItem.GetComponent<plantID>()) != null)
            {
                //If it is a plant pickup...
                if (thisID.ID == usableID)
                {
                    //With a matching ID, destroy it.
                    Debug.Log("Got a hit, destroying");
                    //(Take out it's planted location first to make it reusable)
                    RemoveByID(thisID.ID);
                    thisID.transform.position = new Vector3(-500, 0, 0);
                    //Destroy(thisID.gameObject);
                }
            }
        }
    }



    //Removes a used planted location by plant ID
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

    //Removes a plant pickup at location. Returns the ID of the plant so other clients can
    //remove it as well
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
