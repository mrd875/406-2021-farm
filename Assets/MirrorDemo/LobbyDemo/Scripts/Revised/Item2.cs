using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.Tilemaps;
using UnityEngine;
using UnityEngine.UI;
using Mirror;

public class Item2 : NetworkBehaviour
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
    
    private Tilemap gl;
    private bool canSwap;

    void Start()
    {
        gl = GameObject.FindGameObjectWithTag("GameGrid").GetComponent<Tilemap>();
    }


    void OnMouseDown()
    {
        //if (pickup_allowed)
        //{
        AddItem();
        Debug.Log("Clicked on item");
        // AddItem();
        //}
    }



    private void OnTriggerEnter2D(Collider2D collision)
    {
        // If player one walks over item
        if (collision.CompareTag("Player"))
        {
            pickup_allowed = true;
            Debug.Log("pick me up");
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        // If player one moves away from item
        if (collision.CompareTag("Player"))
        {
            pickup_allowed = false;
        }
        
    }

    public void ChangeToInventorySprite()
    {
        this.GetComponent<SpriteRenderer>().sprite = InventorySprite;
        this.GetComponent<SpriteRenderer>().color = Color.white;
    }

    private void AddItem()
    {
        Debug.Log("Attempting to add item");
        Sprite oldSprite = this.GetComponent<SpriteRenderer>().sprite;
        Color oldColor = this.GetComponent<SpriteRenderer>().color;
        ChangeToInventorySprite();
        // attempt to add item. Canswap takes in a bool returned by AddItem indicating its success
        //canSwap = .AddItem(this);
        GameObject.Find("GameObject_NetworkPlayer(Clone)").GetComponent<PlayerInventory2>().itemClicked = this;
        if (GameObject.Find("GameObject_NetworkPlayer(Clone)").GetComponent<PlayerInventory2>().itemClicked != null)
            canSwap = true;
        else
            canSwap = false;
        if (!canSwap)
        {
            this.GetComponent<SpriteRenderer>().sprite = oldSprite;
            this.GetComponent<SpriteRenderer>().color = oldColor;
        }
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
        else
        {
            switch (itemName)
            {
                case "Shovel":
                    Vector2 mousePos = Input.mousePosition;
                    Vector2 worldPosition2D = Camera.main.ScreenToWorldPoint(mousePos);
                    Vector3 worldPosition = new Vector3(worldPosition2D.x, worldPosition2D.y, transform.position.z);
                    Vector3Int worldPos = gl.WorldToCell(worldPosition);

                    // locally update our tile
                    gl.SetTile(worldPos, null);

                    // tell the server to tell other clients about our click
                    CmdSetTile(worldPos);

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

    [Command]
    private void CmdSetTile(Vector3Int v)
    {
        // tell other clients about our click
        RpcSetTile(v);
    }

    [ClientRpc]
    private void RpcSetTile(Vector3Int v)
    {
        // update our tile
        gl.SetTile(v, null);
    }

}
