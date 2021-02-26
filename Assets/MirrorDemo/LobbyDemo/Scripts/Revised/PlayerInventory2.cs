﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.Tilemaps;
using UnityEngine;
using TMPro;
using Mirror;

public class PlayerInventory2 : NetworkBehaviour
{
    // ToDo: Move player inventory elements from player data to here. static?

    // Array of linked lists, each indice contains an item slot
    public LinkedList<Item2>[] itemSlots;

    // A pointer to the slot the player has control over
    public LinkedList<Item2> selectedSlot;
    public int selectedSlotNumber;
    public GameObject selectedSlotUI;

    //Player global funds and money stuff
    public int money = 100;
    public bool inBinRange = false;
    
    private Tilemap gl;

    void Start()
    {
        itemSlots = new LinkedList<Item2>[5];
        itemSlots[0] = new LinkedList<Item2>();
        itemSlots[1] = new LinkedList<Item2>();
        itemSlots[2] = new LinkedList<Item2>();
        itemSlots[3] = new LinkedList<Item2>();
        itemSlots[4] = new LinkedList<Item2>();

        selectedSlotNumber = 0;

        gl = GameObject.FindGameObjectWithTag("GameGrid").GetComponent<Tilemap>();
    }

    void Update()
    {
        selectedSlot = itemSlots[selectedSlotNumber];
        selectedSlotUI = GameObject.Find("Slot1UI");
    }

    public bool AddItem(Item2 item)
    {
        // slotToAdd will remain -1 until end only if inventory is full
        int slotToAdd = -1; 

        // Either find the lowest slot number, or the slot number thats item matches the item if it is stackable
        for (int i = 0; i < itemSlots.Length; i++)
        {
            if ((slotToAdd == -1) && (itemSlots[i].Count == 0))
                slotToAdd = i;
            if (item.is_stackable)
            {
                if ((itemSlots[i].Count > 0) && (item.itemName == itemSlots[i].First.Value.itemName))
                    slotToAdd = i;
            }
            else if (slotToAdd != -1)
            {
                break;
            }
        }

        if (slotToAdd != -1)
        {
            itemSlots[slotToAdd].AddFirst(item);
            //item.transform.position = new Vector3(-500, 0, 0);
        }

        // Update inventory GUI on screen
        switch (slotToAdd)
        {
            case 0:
                UpdateUI("Slot1UI", item, 0);
                return true;

            case 1:
                UpdateUI("Slot2UI", item, 1);
                return true;

            case 2:
                UpdateUI("Slot3UI", item, 2);
                return true;

            case 3:
                UpdateUI("Slot4UI", item, 3);
                return true;

            case 4:
                UpdateUI("Slot5UI", item, 4);
                return true;
            case -1:
                // inventory full
                return false;

        }
        return false;
    }

    public void UpdateUI(string slotName, Item2 item, int slotNumber)
    {
        GameObject.Find(slotName).transform.GetChild(0).GetComponent<Image>().sprite = item.gameObject.GetComponent<SpriteRenderer>().sprite;
        Debug.Log("Adding to slot " + slotNumber.ToString());
        Debug.Log("Active Slot: " + selectedSlotNumber.ToString());

        //Check if item is being added to active slot. Adjust color appropriately. 
        if (selectedSlotNumber == slotNumber)
        {
            Debug.Log("Inside");
            Color itemColor = item.gameObject.GetComponent<SpriteRenderer>().color;
            GameObject.Find(slotName).transform.GetChild(0).GetComponent<Image>().color = new Color(itemColor.r, itemColor.g, itemColor.b, 0.5f);
        }
        else
        {
            GameObject.Find(slotName).transform.GetChild(0).GetComponent<Image>().color = item.gameObject.GetComponent<SpriteRenderer>().color;
        }

        if (item.is_stackable)
            GameObject.Find(slotName).transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = "" + itemSlots[slotNumber].Count;
        //WorldData.RemovePlantedLocation(WorldData.diggableLayer.WorldToCell(item.transform.localPosition));
        item.transform.position = new Vector3(-500, 0, 0);
    }

    public void DropItem()
    {
        Debug.Log("Attempting to drop");
        if (selectedSlot.Count > 0)
        {
            Item2 droppedItem = selectedSlot.First.Value;
            droppedItem.GetComponent<SpriteRenderer>().enabled = true;
            droppedItem.transform.position = gameObject.transform.position;
            selectedSlot.Remove(droppedItem);
            if (selectedSlot.Count == 0)
            {
                Image image = selectedSlotUI.transform.GetChild(0).GetComponent<Image>();   // For shorter reference
                image.color = new Color(image.color.r, image.color.g, image.color.b, 0f);   // Remove visibility of item icon by setting alpha to 0
                selectedSlotUI.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = ""; // clear text
            }
            else
                selectedSlotUI.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = "" + itemSlots[selectedSlotNumber].Count;
        }
    }

    public void UseSelectedItem(Vector2 location)
    {
        if (selectedSlot.Count != 0)
        {
            Item2 item = selectedSlot.First.Value;
            // if item is a seed it adds a tile based on the editor
            if (item.is_seed)
            {
/*                Vector3Int pos = WorldData.diggableLayer.WorldToCell(location);//PlayerData.player.transform.position);

                if ((WorldData.diggableLayer.GetTile(pos) == null) && (WorldData.plantableLayer.GetTile(pos) != null) && (WorldData.CheckPlantedLocation(pos)))
                {
                    // Place item in middle of cell, track planted location
                    Instantiate(actionPrefab, WorldData.plantableLayer.CellToWorld(pos), Quaternion.identity);
                    WorldData.AddPlantedLocation(pos);

                    return true;
                }*/
            }
            else
            {
                switch (item.itemName)
                {
                    case "Shovel":
                        // get the position of the click

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
                if (item.itemName.Substring(0, 8) == "Sellable")
                {
                    if (PlayerData.inBinRange)
                    {
                        Sellable sellComp = GetComponent<Sellable>();
                        sellComp.SellPlant();
                    }
                }
            }

            ItemUsed(); // should be called after an item is successfully used
        }
    }

    public void ItemUsed()
    {
        // Consumption
        if (selectedSlot.First.Value.is_consumable)
        {
            Item2 consumedItem = selectedSlot.First.Value;
            selectedSlot.Remove(consumedItem);
            Destroy(consumedItem);
            if (selectedSlot.Count == 0)
            {
                Image image = selectedSlotUI.transform.GetChild(0).GetComponent<Image>();   // For shorter reference
                image.sprite = null;
                image.color = new Color(1, 1, 1, 0.5f);
                selectedSlotUI.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = ""; // clear text
            }
            else
                selectedSlotUI.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = "" + itemSlots[selectedSlotNumber].Count;
        }
    }

    public void AddMoney(int value)
    {
        if (money + value > 0)
        {
            money += value;
        }
        else
        {
            money = 0;
        }

        //Update money text
        UpdateMoney moneyText = GameObject.FindObjectOfType<UpdateMoney>();
        moneyText.UpdateMoneyText();
    }

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