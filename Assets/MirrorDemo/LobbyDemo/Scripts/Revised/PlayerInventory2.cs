using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.Tilemaps;
using UnityEngine;
using TMPro;
using Mirror;
using UnityEngine.PlayerLoop;

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

    public GameObject shovelPrefab;

    void Start()
    {
        itemSlots = new LinkedList<Item2>[5];
        itemSlots[0] = new LinkedList<Item2>();
        itemSlots[1] = new LinkedList<Item2>();
        itemSlots[2] = new LinkedList<Item2>();
        itemSlots[3] = new LinkedList<Item2>();
        itemSlots[4] = new LinkedList<Item2>();

        selectedSlotNumber = 0;
        selectedSlot = itemSlots[selectedSlotNumber];
        selectedSlotUI = GameObject.Find("Slot1UI");

        // Add shovel from start
        GameObject shovel = Instantiate(shovelPrefab, Vector2.zero, Quaternion.identity);
        AddItem(shovel.GetComponent<Item2>());
    }

    void Update()
    {
        selectedSlot = itemSlots[selectedSlotNumber];
    }

    public bool AddItem(Item2 item)
    {
        // slotToAdd will remain -1 until end only if inventory is full
        int slotToAdd = -1;

        // Either find the lowest slot number, or the slot number that's item matches the item if it is stackable
        for (int i = 0; i < itemSlots.Length; i++)
        {
            if ((slotToAdd == -1) && (itemSlots[i].Count == 0))
                //But keep looking, as there may be a stackable slot later on
                slotToAdd = i;

            if (item.is_stackable)
            {
                if ((itemSlots[i].Count > 0) && (item.itemName == itemSlots[i].First.Value.itemName))
                {
                    slotToAdd = i;
                    //No matter what add it to the slot of existing items. No need to look further
                    break;
                }
            }
        }
        if (slotToAdd != -1)
        {
            itemSlots[slotToAdd].AddFirst(item);
            item.transform.gameObject.GetComponent<SpriteRenderer>().sprite = item.InventorySprite;
            item.transform.position = new Vector3(-500, 0, 0);
        }
        // Update inventory UI on screen
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

        //Check if item is being added to active slot. Adjust color appropriately. 
        if (selectedSlotNumber == slotNumber)
        {
            Color itemColor = item.gameObject.GetComponent<SpriteRenderer>().color;
            //Fade out a bit in active slot
            GameObject.Find(slotName).transform.GetChild(0).GetComponent<Image>().color = new Color(itemColor.r, itemColor.g, itemColor.b, 0.5f);
        }
        else
        {
            GameObject.Find(slotName).transform.GetChild(0).GetComponent<Image>().color = item.gameObject.GetComponent<SpriteRenderer>().color;
        }

        if (item.is_stackable)
            GameObject.Find(slotName).transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = "" + itemSlots[slotNumber].Count;
        //Destroy(item.transform.gameObject);
        item.transform.position = new Vector3(-500, 0, 0);
    }

    public bool DropItem()
    {
        Debug.Log("Attempting to drop");
        if (selectedSlot.Count > 0)
        {
            // Do not allow player to drop their shovel
            if (selectedSlot.First.Value != null && selectedSlot.First.Value.itemName == "Shovel")
                return false;

            Item2 droppedItem = selectedSlot.First.Value;
            droppedItem.GetComponent<SpriteRenderer>().enabled = true;
            //droppedItem.transform.position = gameObject.transform.position;   Done in command script
            selectedSlot.Remove(droppedItem);
            if (selectedSlot.Count == 0)
            {
                Image image = selectedSlotUI.transform.GetChild(0).GetComponent<Image>();   // For shorter reference
                image.color = new Color(image.color.r, image.color.g, image.color.b, 0f);   // Remove visibility of item icon by setting alpha to 0
                selectedSlotUI.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = ""; // clear text
            }
            else
                selectedSlotUI.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = "" + itemSlots[selectedSlotNumber].Count;
            return true;
        }
        return false;
    }

    public void UseSelectedItem(Vector2 location)
    {
        if (selectedSlot.Count != 0)
        {
            Item2 item = selectedSlot.First.Value;
            Vector3Int tileCoord = WorldData2.p1DiggableLayer.WorldToCell(location);  //Get cell location
            // if item is a seed it adds a tile based on the editor
            if (item.is_seed)
            {
                if ((WorldData2.p1DiggableLayer.GetTile(tileCoord) == null && WorldData2.p2DiggableLayer.GetTile(tileCoord) == null)
                    && (WorldData2.plantableLayer.GetTile(tileCoord) != null) && (WorldData2.CheckPlantedLocation(tileCoord)))
                {
                    float growthRate = PlayerData2.localGrowSpeed;

                    //Extract vegetable name by taking off last 5 characters (" seed")
                    string vegetableString = item.itemName.Substring(0, item.itemName.Length - 5);

                    // Place item in middle of cell, track planted location. All handled by world data
                    bool plantAttempt = WorldData2.AddPlantedLocation(tileCoord, vegetableString, growthRate);

                    //On succesful plant, tell other clients to add a plant at that location as well
                    if (plantAttempt)
                    {
                        SoundControl.PlayPlantSound();
                        Debug.Log("here");
                        //Tell others to add the plant
                        Debug.Log("Sending through growth rate of " + growthRate.ToString());
                        CmdPlantSeed(tileCoord, vegetableString, growthRate);
                        ItemUsed();
                    }
                }
            }
            else
            {
                switch (item.itemName)
                {
                    case "Shovel":
                        // locally update our tile
                        WorldData2.p1DiggableLayer.SetTile(tileCoord, null);
                        WorldData2.p2DiggableLayer.SetTile(tileCoord, null);
                        SoundControl.PlayShovelSound();
                        // tell the server to tell other clients about our click
                        if (isServer)
                            RpcSetTile(tileCoord);
                        else
                            CmdSetTile(tileCoord);
                        break;

                    case "BearTrap":
                        //selectedItemObject = selectedItem.actionPrefab;
                        if (isServer)
                            RpcSetBearTrap(location, gameObject.tag);
                        else
                            CmdSetBearTrap(location, gameObject.tag);
                        break;
                }
            }
        }
    }

    //Called by sell bin on player click when in range
    public void SellItem()
    {
        Item2 item = selectedSlot.First.Value;
        //Sellables work differently
        if (item.itemName.Length > 8 && item.itemName.Substring(0, 8) == "Sellable")
        {
            Sellable sellComp = item.GetComponent<Sellable>();
            if (sellComp.SellPlant())
            {
                SoundControl.PlayMoneySound();
                ItemUsed();
            }
        }
    }



    public void ItemUsed()
    {
        // Consumption
        if (selectedSlot.First.Value.is_consumable)
        {
            Item2 consumedItem = selectedSlot.First.Value;
            selectedSlot.Remove(consumedItem);
            //Destroy(consumedItem);
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


    [Command]
    private void CmdPlantSeed(Vector3Int location, string plantName, float growthRate)
    {
        Debug.Log("Still have growth rate of " + growthRate.ToString());
        RpcPlantSeed(location, plantName, growthRate);
    }

    [ClientRpc]
    private void RpcPlantSeed(Vector3Int location, string plantName, float growthRate)
    {
        Debug.Log("Have plant growth of: " + growthRate);
        WorldData2.AddPlantedLocation(location, plantName, growthRate);
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
        WorldData2.p1DiggableLayer.SetTile(v, null);
        WorldData2.p2DiggableLayer.SetTile(v, null);
    }


    [Command]
    private void CmdSetBearTrap(Vector2 location, string userTag)
    {
        RpcSetBearTrap(location, tag);
    }

    [ClientRpc]
    private void RpcSetBearTrap(Vector2 location, string userTag)
    {
        GameObject bearTrap = Instantiate(ObjectData.bearTrapPrefab, location, Quaternion.identity);
        bearTrap.GetComponent<BearTrap>().trapOwnerTag = gameObject.tag;
        if (userTag == PlayerData2.localPlayer.tag)
            ItemUsed();
    }

}