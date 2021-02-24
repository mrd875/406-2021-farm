using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine.Tilemaps;
using UnityEngine;
using UnityEngine.UI;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;

public class PlayerInteraction : MonoBehaviour
{
    // Item use key
    public KeyCode itemKey = KeyCode.E;

    //private bool inBinRange = false;

    private int oldSlotNumber = 0;
    private string[] slotNames = new string[]{"Slot1UI", "Slot2UI","Slot3UI", "Slot4UI", "Slot5UI"};

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

            //Scroll to change items
            if (Input.mouseScrollDelta.y > 0)
            {
                if (oldSlotNumber < 4)
                {
                    SetSlot(slotNames[oldSlotNumber + 1], oldSlotNumber + 1);
                }
                else
                {
                    SetSlot(slotNames[0], 0);
                }
            }

            if (Input.mouseScrollDelta.y < 0)
            {
                if (oldSlotNumber > 0)
                {
                    SetSlot(slotNames[oldSlotNumber -1], oldSlotNumber - 1);
                }
                else
                {
                    SetSlot(slotNames[4], 4);
                }
            }

            // Change Item Cursor with keys 1-5
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                SetSlot("Slot1UI", 0);
            }
            if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                SetSlot("Slot2UI", 1);
            }
            if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                SetSlot("Slot3UI", 2);
            }
            if (Input.GetKeyDown(KeyCode.Alpha4))
            {
                SetSlot("Slot4UI", 3);
            }
            if (Input.GetKeyDown(KeyCode.Alpha5))
            {
                SetSlot("Slot5UI", 4);
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

    //Changes to the given slot. slotName must match exactly to the scene name of UI element.
    public void SetSlot(string slotName, int slotNumber)
    {
        //There is something in the slot you are leaving. Restore opacity
        if (PlayerData.itemSlots[oldSlotNumber].Count != 0)
        {
            PlayerData.selectedSlotUI.transform.GetChild(0).GetComponent<Image>().color = new Color(1, 1, 1, 1);
        }
        //Leaving an empty slot. Make it invisible
        else
        {
            PlayerData.selectedSlotUI.transform.GetChild(0).GetComponent<Image>().color = new Color(1, 1, 1, 0);
        }
        //Store data to check on next slot change
        oldSlotNumber = slotNumber;

        //Change to new item
        PlayerData.selectedSlotNumber = slotNumber;
        PlayerData.selectedSlotUI = GameObject.Find(slotName);
        PlayerData.selectedSlotUI.transform.GetChild(0).GetComponent<Image>().color = new Color(1, 1, 1, 0.5f);
    }

    void Clicked()
    {
        Vector2 mousePos = Input.mousePosition;
        Vector2 worldPosition2D = Camera.main.ScreenToWorldPoint(mousePos);
        Vector3 worldPosition = new Vector3(worldPosition2D.x, worldPosition2D.y, this.transform.position.z);
        if (PlayerData.itemClicked != null)
        {
            PlayerData.AddItem(PlayerData.itemClicked);
            PlayerData.itemClicked = null;
        }
        else if (PlayerData.selectedSlot.First.Value != null && PlayerData.interactionRadius.bounds.Contains(worldPosition))
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
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Bin"))
        {
            PlayerData.inBinRange = false;
        }
    }

}
