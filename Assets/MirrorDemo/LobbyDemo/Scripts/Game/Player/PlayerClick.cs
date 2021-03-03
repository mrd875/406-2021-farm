using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;
using Mirror;

using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;

public class PlayerClick : NetworkBehaviour
{
    public Tile highlightTile;
    public float interactionRange = 2f; // range from player to cursor for which player can interact
    Vector3Int previousTileCoordinate;

    private bool canInteract = false;

    // Inventory script belonging to this.gameObject
    private PlayerInventory2 inventory;

    private int oldSlotNumber = 0;
    private string[] slotNames = new string[] { "Slot1UI", "Slot2UI", "Slot3UI", "Slot4UI", "Slot5UI" };

    private LayerMask whatIsItem;

    

    private void Start()
    {
        highlightTile.color = new Color(0f, 0.5f, 1f, 0.5f); // default color (rgba)
        inventory = gameObject.GetComponent<PlayerInventory2>();
        whatIsItem = LayerMask.GetMask("Item");
    }

    private void Update()
    {
        // only listen for clicks on our own game object.
        if (!hasAuthority)
            return;

        // Get mouse coordinates (for highlight tile)
        Vector2 mousePos = Input.mousePosition;
        Vector2 mouseWorldPos = Camera.main.ScreenToWorldPoint(mousePos);
        Vector3Int tileCoordinate = WorldData2.highlighter.WorldToCell(mouseWorldPos);

        // Only select one of the following modes for the tiles to be highlighed
        // cursorHighlightMode1(tileCoordinate);  // Cursor turns red when out of range
        cursorHighlightMode2(tileCoordinate);   // Cursor disappears when out of range

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
                SetSlot(slotNames[oldSlotNumber - 1], oldSlotNumber - 1);
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
            CmdDropItem(inventory.selectedSlot.First.Value.GetComponent<Item2>(), gameObject.transform.position);
            inventory.DropItem();
        }

        if (Input.GetMouseButtonDown(0) && canInteract)
        {
            Vector2 worldPosition2D = Camera.main.ScreenToWorldPoint(mousePos);
            Vector3 worldPosition = new Vector3(worldPosition2D.x, worldPosition2D.y, this.transform.position.z);
            RaycastHit2D hit = Physics2D.Raycast(worldPosition2D, Vector2.zero, 1.0f, whatIsItem);
            //Debug.Log("Hit item name: "+hit.collider.gameObject.name);

            if (hit.collider != null && hit.collider.gameObject.GetComponent<Item2>() != null)
            {
                inventory.AddItem(hit.collider.gameObject.GetComponent<Item2>());
                CmdAddItem(hit.collider.gameObject.GetComponent<Item2>());
            }
            // The if selectedSlot.First == null, expression will evaluate to null while ignoring anything after the ':'so null exception will not be thrown
            else if ((inventory.selectedSlot.First == null ? null : inventory.selectedSlot.First.Value) != null)
            {
                inventory.UseSelectedItem(worldPosition);
            }
            else
            {
                Debug.Log("false");
            }

        }
    }
 
    // The tile highlight cursor turns red when out of range
    // param name=tileCoordinate: Tile coordinate on the grid to be highlighted
    private void cursorHighlightMode1(Vector3Int tileCoordinate)
    {
        if (Vector2.Distance(gameObject.transform.position, WorldData2.highlighter.CellToWorld(tileCoordinate)) > interactionRange)
        {
            highlightTile.color = new Color(1f, 0f, 0f, 0.5f); // turn red
            WorldData2.highlighter.SetTile(tileCoordinate, null);
            WorldData2.highlighter.SetTile(tileCoordinate, highlightTile);
            canInteract = false;
        }
        else
        {
            highlightTile.color = new Color(0.0f, 0.5f, 1f, 0.5f);
            WorldData2.highlighter.SetTile(tileCoordinate, null);
            WorldData2.highlighter.SetTile(tileCoordinate, highlightTile);
            canInteract = true;
        }

        if (tileCoordinate != previousTileCoordinate)
        {
            WorldData2.highlighter.SetTile(previousTileCoordinate, null);
            WorldData2.highlighter.SetTile(tileCoordinate, highlightTile);
            previousTileCoordinate = tileCoordinate;
        }
    }

    // The tile highlight cursor disappears when out of range
    // param name=tileCoordinate: Tile coordinate on the grid to be highlighted
    private void cursorHighlightMode2(Vector3Int tileCoordinate)
    {
        // Check if the center of the tile cursor is on is in interaction range
        if (Vector2.Distance(gameObject.transform.position, WorldData2.highlighter.CellToWorld(tileCoordinate)) < interactionRange)
        {
            // Highlight the tile cursor is on
            if (tileCoordinate != previousTileCoordinate)
            {
                WorldData2.highlighter.SetTile(previousTileCoordinate, null);
                WorldData2.highlighter.SetTile(tileCoordinate, highlightTile);
                previousTileCoordinate = tileCoordinate;
            }
            canInteract = true;
        }
        else
        {
            if (canInteract)
            {
                WorldData2.highlighter.SetTile(previousTileCoordinate, null);
            }
            canInteract = false;
        }
    }

    //Changes to the given slot. slotName must match exactly to the scene name of UI element.
    public void SetSlot(string slotName, int slotNumber)
    {
        //There is something in the slot you are leaving. Restore opacity
        if (inventory.itemSlots[oldSlotNumber].Count != 0)
        {
            inventory.selectedSlotUI.transform.GetChild(0).GetComponent<Image>().color = new Color(1, 1, 1, 1);
        }
        //Leaving an empty slot. Make it invisible
        else
        {
            inventory.selectedSlotUI.transform.GetChild(0).GetComponent<Image>().color = new Color(1, 1, 1, 0);
        }
        //Store data to check on next slot change
        oldSlotNumber = slotNumber;

        //Change to new item
        inventory.selectedSlotNumber = slotNumber;
        inventory.selectedSlotUI = GameObject.Find(slotName);
        inventory.selectedSlotUI.transform.GetChild(0).GetComponent<Image>().color = new Color(1, 1, 1, 0.5f);
    }



    [Command]
    private void CmdAddItem(Item2 i)
    {
        RpcAddItem(i);
    }

    [Command]
    private void CmdDropItem(Item2 i, Vector2 v)
    {
        RpcDropItem(i, v);
    }


    [ClientRpc]
    private void RpcAddItem(Item2 i)
    {
        i.transform.position = new Vector3(-500, 0, 0);
    }

    [ClientRpc]
    private void RpcDropItem(Item2 i, Vector2 v)
    {
        i.transform.position = v;
    }
}
