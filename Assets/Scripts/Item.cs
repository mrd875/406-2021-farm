﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine.Tilemaps;
using UnityEngine;

public class Item : MonoBehaviour
{
    public string itemName;
    public Tile actionTile;
    public GameObject actionPrefab;

    [HideInInspector]
    public bool pickup_allowed = false;
    public bool is_stackable = true;
    public bool is_consumable = true;
    public bool is_seed = false;

    private bool canSwap;

    void Update()
    {
        // Grab item button
        if (Input.GetKeyDown(KeyCode.F) && pickup_allowed)
        {
            AddItem();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // If player one walks over item
        if (collision.CompareTag("PlayerOne"))
        {
            pickup_allowed = true;
            Debug.Log("pick me up");
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        // If player one moves away from item
        if (collision.CompareTag("PlayerOne"))
        {
            pickup_allowed = false;
            Debug.Log("pick me up");
        }
        
    }

    private void AddItem()
    {
        // attempt to add item. Canswap takes in a bool returned by AddItem indicating its success
        canSwap = PlayerData.AddItem(this);
    }

    public bool UseItem(Vector2 location)
    {
        // if item is a seed it adds a tile based on the editor
        if (is_seed)
        {
            Vector3Int pos = WorldData.diggableLayer.WorldToCell(location);//PlayerData.player.transform.position);
            if (actionTile == null)
                Debug.Log("Error: actionTile is not set for given seed");

            if ((WorldData.diggableLayer.GetTile(pos) == null) && (WorldData.plantableLayer.GetTile(pos) != null) && (WorldData.CheckPlantedLocation(pos)))
            {
                // Place item in middle of cell, track planted location
                Instantiate(actionPrefab, WorldData.plantableLayer.CellToWorld(pos), Quaternion.identity);
                WorldData.AddPlantedLocation(pos);

                return true;
            }
        }

        // For items that require specific functions
        else {
            switch (itemName)
            {
                case "Shovel":
                    // Shovel removes a tile off the top layer of the grid, tile should be flagged as diggable; for example, a shovel shouldn't be allowd to dig through concrete
                    // This can be changed so that it adds a dirt tile on top instead, or it replaces a grass tile with a dirt one with relative ease
                    Vector3Int tileCoordinate = WorldData.diggableLayer.WorldToCell(location);//PlayerData.player.transform.position);
                    if (PlayerData.userArea.OverlapPoint(location))
                    {
                        if (WorldData.diggableLayer.GetTile(tileCoordinate) != null)
                        {
                            WorldData.diggableLayer.SetTile(tileCoordinate, null);
                            return true;
                        }
                    }
                    break;
                
                case "Sellable":
                    if (PlayerData.inBinRange)
                    {
                        Sellable sellComp = GetComponent<Sellable>();
                        sellComp.SellPlant();
                        return true;
                    }
                    break;


            }
        }
        return false;
    }
}
