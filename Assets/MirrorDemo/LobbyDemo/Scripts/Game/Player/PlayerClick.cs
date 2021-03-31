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
using Quaternion = UnityEngine.Quaternion;

public class PlayerClick : NetworkBehaviour
{
    public Tile highlightTile;

    public float interactionRange = 2f; // range from player to cursor for which player can interact
    Vector3Int previousTileCoordinate;

    
    public bool canInteract = true; // If cursor is in interaction range. 
    public bool canInteractWithTile = true; // If interaction range reaches the center of the tile that the mouse is on

    // Inventory script belonging to this.gameObject
    private PlayerInventory2 inventory;
    private GameObject selectedItemSpriteGO;    // place-holder for drawing items at cursor; displayed placable item before it is placed
    private SpriteRenderer selectedItemSR;
    public bool canPlaceItem = true;

    private int oldSlotNumber = 0;
    private string[] slotNames = new string[] { "Slot1UI", "Slot2UI", "Slot3UI", "Slot4UI", "Slot5UI" };

    private LayerMask whatIsInteractable;
    public GameObject highlightedInteractable;

    public Sprite inventoryNormalSprite;
    public Sprite inventorySelectedSprite;

    private void Start()
    {
        highlightTile.color = new Color(0f, 0.5f, 1f, 0.5f); // default color (rgba)
        inventory = gameObject.GetComponent<PlayerInventory2>();
        whatIsInteractable = LayerMask.GetMask("Interactable");

        if (hasAuthority)
        {
            selectedItemSpriteGO = new GameObject();
            selectedItemSpriteGO.tag = "Cursor";
            CircleCollider2D cc = selectedItemSpriteGO.AddComponent<CircleCollider2D>();
            cc.isTrigger = true;
            selectedItemSR = selectedItemSpriteGO.AddComponent<SpriteRenderer>();
            selectedItemSR.sortingOrder = 0;
            selectedItemSR.sortingLayerName = "Item";
        }
    }

    private void Update()
    {
        // only listen for clicks on our own game object.
        if (!hasAuthority)
            return;


        // highlight interactable at mouseover if in range to grab
        if (highlightedInteractable != null)
            if (canInteract)
                highlightedInteractable.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 0.5f);
            else
                highlightedInteractable.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1);

        // Get mouse coordinates (for highlight tile)
        Vector2 mousePos = Input.mousePosition;
        Vector2 mouseWorldPos = Camera.main.ScreenToWorldPoint(mousePos);
        Vector3Int tileCoordinate = WorldData2.highlighter.WorldToCell(mouseWorldPos);


        if (Vector2.Distance(gameObject.transform.position, mouseWorldPos) < interactionRange)
            canInteract = true;
        else
            canInteract = false;



        // If placable item is selected form inventory, draw item at cursor
        if (inventory.selectedSlot.First != null && inventory.selectedSlot.First.Value.itemName == "BearTrap")
        {
            WorldData2.highlighter.SetTile(tileCoordinate, null);
            DrawItemAtCursor(mouseWorldPos);
        }
        // Highlight tile at mouseover
        else
        {
            Color color = selectedItemSR.material.color;
            color.a = 0;
            selectedItemSR.material.color = color;
            CursorHighlight(tileCoordinate);   // Cursor disappears when out of range
        }

        //Scroll to change items
        if (Input.mouseScrollDelta.y < 0)
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
        if (Input.mouseScrollDelta.y > 0)
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

        // Interact
        if (Input.GetMouseButtonDown(0) && canInteract)
        {
            ShopSystem shop;    // used to store shop script if exists on interactable    

            // Check if interactable object was clicked
            if (highlightedInteractable != null)
            {
                // Check if interactable object is an item
                if (highlightedInteractable.GetComponent<Item2>() != null)
                {
                    highlightedInteractable.gameObject.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1);
                    Item2 itemToAdd = highlightedInteractable.GetComponent<Item2>();
                    highlightedInteractable = null;
                    bool couldAdd = inventory.AddItem(itemToAdd);

                    if (couldAdd)
                    {
                        SoundControl.PlayHarvestSound();
                        //Remove the pickup and planted location from user and all other clients
                        int removedID = WorldData2.RemovePlantedLocation(WorldData2.p1DiggableLayer.WorldToCell(mouseWorldPos));
                        if (itemToAdd.gameObject.GetComponent<plantID>() != null)
                        {
                            removedID = itemToAdd.gameObject.GetComponent<plantID>().ID;
                        }
                        CmdAddItem(itemToAdd, removedID);
                    }
                }
                // Check if interactable object is a seed bin
                else if (highlightedInteractable.GetComponent<SeedBin>() != null)
                {
                    SoundControl.PlayButtonSound();
                    inventory.AddItem(highlightedInteractable.GetComponent<SeedBin>().seed);
                }
                // Check if interactable object is a sell bin
                else if (highlightedInteractable.GetComponent<SellingBin>() != null)
                {
                    inventory.SellItem();
                }
                // Check if interactable object is the shop
                else if ((shop = highlightedInteractable.GetComponent<ShopSystem>()) != null)
                {
                    shop.OpenShopWindow();
                }
            }
            // Use item
            else if (inventory.selectedSlot.First != null)
            {
                Debug.Log("Using Item");
                if (inventory.selectedSlot.First.Value.itemName == "BearTrap")
                {
                    if (canPlaceItem)
                        inventory.UseSelectedItem(mouseWorldPos);
                }
                else
                    // if item interacts with tiles (e.g. shovel and seeds) 
                    if (canInteractWithTile)
                        inventory.UseSelectedItem(mouseWorldPos);
            }
            // No actions
            else
            {
                // Can test new functions here, Activates when empty item slot is selected when click in radius occurs
                Debug.Log("false");
            }
        }
    }

    //Changes to the given slot. slotName must match exactly to the scene name of UI element.
    public void SetSlot(string slotName, int slotNumber)
    {
        //There is something in the slot you are leaving. Restore opacity
        if (inventory.itemSlots[oldSlotNumber].Count != 0)
        {
            inventory.selectedSlotUI.transform.GetChild(0).GetComponent<Image>().color = new Color(1, 1, 1, 1);
            inventory.selectedSlotUI.transform.GetComponent<Image>().sprite = inventoryNormalSprite;
        }
        //Leaving an empty slot. Make it invisible
        else
        {
            inventory.selectedSlotUI.transform.GetChild(0).GetComponent<Image>().color = new Color(1, 1, 1, 0);
            inventory.selectedSlotUI.transform.GetComponent<Image>().sprite = inventoryNormalSprite;
        }
        //Store data to check on next slot change
        oldSlotNumber = slotNumber;

        //Change to new item
        inventory.selectedSlotNumber = slotNumber;
        GameObject newSlot = GameObject.Find(slotName);
        inventory.selectedSlotUI = newSlot;
        inventory.selectedSlotUI.transform.GetChild(0).GetComponent<Image>().color = new Color(1, 1, 1, 0.5f);
        inventory.selectedSlotUI.transform.GetComponent<Image>().sprite = inventorySelectedSprite;
    }

    private void DrawItemAtCursor(Vector2 mouseWorldPos)
   {
        Color color = selectedItemSR.material.color;
        if (!canPlaceItem)
        {
            color.g = 0;
            color.b = 0;
            selectedItemSR.material.color = color;
        }
        else
        {
            color.g = 1;
            color.b = 1;
            selectedItemSR.material.color = color;
        }
        selectedItemSR.sprite = inventory.selectedSlot.First.Value.gameObject.GetComponent<SpriteRenderer>().sprite;
        // Check if the center of the tile cursor is on is in interaction range
        if (canInteract)
        {
            // Remove highlight if an interactable is present at cursor
            if (highlightedInteractable != null)
            {
                color.a = 0;
                selectedItemSR.material.color = color;
            }
            else
            {
                color.a = 0.7f;
                selectedItemSR.material.color = color;
                selectedItemSpriteGO.transform.position = mouseWorldPos;
            }
        }
        else
        {
            color.a = 0;
            selectedItemSR.material.color = color;
        }
    }

    // The tile highlight cursor disappears when out of range
    // param name=tileCoordinate: Tile coordinate on the grid to be highlighted
    private void CursorHighlight(Vector3Int tileCoordinate)
    {
        // Check if the center of the tile cursor is on is in interaction range
        if (Vector2.Distance(gameObject.transform.position, WorldData2.highlighter.CellToWorld(tileCoordinate)) < interactionRange)
        {
            canInteractWithTile = true;
            // Remove highlight if an interactable is present at cursor
            if (highlightedInteractable != null)
            {
                WorldData2.highlighter.SetTile(tileCoordinate, null);
            }
            else if (WorldData2.plantableLayer.GetTile(tileCoordinate) != null)
            {
                WorldData2.highlighter.SetTile(tileCoordinate, highlightTile);
            }

            // Highlight new tiles that the cursor moves to
            if (tileCoordinate != previousTileCoordinate)
            {
                WorldData2.highlighter.SetTile(previousTileCoordinate, null);
                if (WorldData2.plantableLayer.GetTile(tileCoordinate) != null)
                {
                    WorldData2.highlighter.SetTile(tileCoordinate, highlightTile);
                    previousTileCoordinate = tileCoordinate;
                }
            }
        }
        else
        {
            if (canInteractWithTile)
            {
                WorldData2.highlighter.SetTile(previousTileCoordinate, null);
            }
            canInteractWithTile = false;
        }
    }

    [Command]
    private void CmdAddItem(Item2 i, int ID)
    {
        RpcAddItem(i, ID);
    }

    [ClientRpc]
    private void RpcAddItem(Item2 i, int ID)
    {
        StartCoroutine(PickupDelay(i, ID));

    }

    private IEnumerator PickupDelay(Item2 i, int ID)
    {
        yield return new WaitForSeconds(0.5f);
        //For plant pickups/ dynamically spawned in stuff
        WorldData2.RemoveItemsWithID(ID);

        //for everything else
        if (i != null)
        {
            //Destroy(i.transform.gameObject);
            i.transform.position = new Vector3(-500, 0, 0);
        }

    }

    [Command]
    private void CmdDropItem(Item2 i, Vector2 v)
    {
        RpcDropItem(i, v);
    }

    [ClientRpc]
    private void RpcDropItem(Item2 i, Vector2 v)
    {
        i.transform.position = v;
    }
}