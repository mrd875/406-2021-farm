using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.Tilemaps;
using UnityEngine;
using UnityEngine.UI;

public class Item : MonoBehaviour
{
    public string itemName;
    public Tile actionTile;
    public GameObject actionPrefab;
    public Sprite InventorySprite;

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

    void OnMouseDown()
    {
        if (pickup_allowed)
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

    public void ChangeToInventorySprite()
    {
        this.GetComponent<SpriteRenderer>().sprite = InventorySprite;
        this.GetComponent<SpriteRenderer>().color = Color.white;
    }

    private void AddItem()
    {
        Sprite oldSprite = this.GetComponent<SpriteRenderer>().sprite;
        Color oldColor = this.GetComponent<SpriteRenderer>().color;
        ChangeToInventorySprite();
        // attempt to add item. Canswap takes in a bool returned by AddItem indicating its success
        //canSwap = PlayerData.AddItem(this);
        PlayerData.itemClicked = this;
        if (!canSwap)
        {
            this.GetComponent<SpriteRenderer>().sprite = oldSprite;
            this.GetComponent<SpriteRenderer>().color = oldColor;
        }
        else
        {
            WorldData.RemovePlantedLocation(WorldData.diggableLayer.WorldToCell(this.transform.localPosition));
        }
        
    }

    public bool UseItem(Vector2 worldPos)
    {
        // if item is a seed it adds a tile based on the editor
        if (is_seed)
        {
            Vector3Int cellPos = WorldData.diggableLayer.WorldToCell(worldPos);//PlayerData.player.transform.position);
            if (actionTile == null)
                Debug.Log("Error: actionTile is not set for given seed");

            if ((WorldData.diggableLayer.GetTile(cellPos) == null) && (WorldData.plantableLayer.GetTile(cellPos) != null) && (WorldData.CheckPlantedLocation(cellPos)))
            {
                // Place item in middle of cell, track planted location
                Instantiate(actionPrefab, WorldData.plantableLayer.GetCellCenterWorld(cellPos), Quaternion.identity);
                WorldData.AddPlantedLocation(cellPos);

                return true;
            }
        }

        // For items that require specific functions
        else
        {
            switch (itemName)
            {
                case "Shovel":
                    // Shovel removes a tile off the top layer of the grid, tile should be flagged as diggable; for example, a shovel shouldn't be allowd to dig through concrete
                    // This can be changed so that it adds a dirt tile on top instead, or it replaces a grass tile with a dirt one with relative ease

                    Vector3Int tileCoordinate = WorldData.diggableLayer.WorldToCell(worldPos);//PlayerData.player.transform.position);
                    if (PlayerData.userArea.OverlapPoint(worldPos))
                    {
                        if (WorldData.diggableLayer.GetTile(tileCoordinate) != null)
                        {
                            WorldData.diggableLayer.SetTile(tileCoordinate, null);
                            return true;
                        }
                    }

                    break;

            }

            //Vegetables work differently
            if (itemName.Substring(0, 8) == "Sellable")
            {
                if (PlayerData.inBinRange)
                {
                    Sellable sellComp = GetComponent<Sellable>();
                    sellComp.SellPlant();
                    return true;
                }
            }
        }

        return false;
    }
}
