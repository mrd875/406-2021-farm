using System.Collections;
using System.Collections.Generic;
using UnityEngine.Tilemaps;
using UnityEngine;
using UnityEngine.UI;

public class PlayerInteraction : MonoBehaviour
{
    // Item use key
    public KeyCode itemKey = KeyCode.E;

    private bool inBinRange = false;
    public GameObject sellText;


    public Tile highlightTile;
    Vector3Int previousTileCoordinate;

    [HideInInspector]
    public bool isLocalPlayer = false;
    [HideInInspector]
    public bool inHomeZone = false;

    void Start()
    {
        if (gameObject.tag == PlayerData.localPlayer.tag)
        {
            isLocalPlayer = true;
        }
    }

    void Update()
    {
        if (isLocalPlayer)
        {
            // Get mouse coordinates (for highlight tile)
            Vector2 mousePos = Input.mousePosition;
            Vector2 mouseWorldPos = Camera.main.ScreenToWorldPoint(mousePos);
            Vector3Int tileCoordinate = WorldData.highlighter.WorldToCell(mouseWorldPos);


            if (tileCoordinate != previousTileCoordinate)
            {
                WorldData.highlighter.SetTile(previousTileCoordinate, null);
                WorldData.highlighter.SetTile(tileCoordinate, highlightTile);
                previousTileCoordinate = tileCoordinate;
            }


            // Change Item Cursor with keys 1-5
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                PlayerData.selectedSlotNumber = 0;
                PlayerData.selectedSlotUI = GameObject.Find("Slot1UI");
            }
            if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                PlayerData.selectedSlotNumber = 1;
                PlayerData.selectedSlotUI = GameObject.Find("Slot2UI");
            }
            if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                PlayerData.selectedSlotNumber = 2;
                PlayerData.selectedSlotUI = GameObject.Find("Slot3UI");
            }
            if (Input.GetKeyDown(KeyCode.Alpha4))
            {
                PlayerData.selectedSlotNumber = 3;
                PlayerData.selectedSlotUI = GameObject.Find("Slot4UI");
            }
            if (Input.GetKeyDown(KeyCode.Alpha5))
            {
                PlayerData.selectedSlotNumber = 4;
                PlayerData.selectedSlotUI = GameObject.Find("Slot5UI");
            }

            // Drop Item
            if (Input.GetKeyDown(KeyCode.R))
            {
                PlayerData.DropItem();
            }

            // Use Item
            if (Input.GetKeyDown(itemKey))
            {
                if (PlayerData.selectedSlot.Count > 0)
                {
                    PlayerData.UseSelectedItem(PlayerData.localPlayer.transform.position);
                }
            }

            if (Input.GetMouseButtonDown(0))
            {
                Clicked();
            }
        }
    }

    void Clicked()
    {
        Vector2 mousePos = Input.mousePosition;
        Vector2 worldPosition = Camera.main.ScreenToWorldPoint(mousePos);
        
        if (PlayerData.interactionRadius.bounds.Contains(worldPosition))
        {
            PlayerData.UseSelectedItem(worldPosition);
        }
        else
        {
            Debug.Log("false");
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        // Collision with a player on user's land will teleport them back to their land
        if (inHomeZone)
        {
            if (other.transform.tag == "PlayerOne")
            {
                other.transform.position = WorldData.playerOneSpawnLocation;
            }

            if (other.transform.tag == "PlayerTwo")
            {
                other.transform.position = WorldData.playerTwoSpawnLocation;
            }
                
            if (other.transform.tag == "PlayerThree")
            {
                other.transform.position = WorldData.playerThreeSpawnLocation;
            }
                
            if (other.transform.tag == "PlayerFour")
            {
                other.transform.position = WorldData.playerFourSpawnLocation;
            }
        }
    }

    //Trigger functions to tell when player is in bin range
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Bin"))
        {
            PlayerData.inBinRange = true;
            sellText.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Bin"))
        {
            PlayerData.inBinRange = false;
            sellText.SetActive(false);
        }
    }

}
