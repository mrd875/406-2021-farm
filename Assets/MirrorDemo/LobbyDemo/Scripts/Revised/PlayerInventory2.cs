using System;
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
    // some constants
    private const int SlotCount = 5;
    private const int PosX = 30;
    private const int DefaultSlotWidth = 105;
    private const int DefaultPosY = -40;
    private const string InventoryGameObjectName = "Inventory";
    private const string SlotUiPrefabPath = "Prefabs/SlotUI";

    // Array of linked lists, each indice contains an item slot
    public LinkedList<Item2>[] ItemSlots;

    // A pointer to the slot the player has control over
    public int SelectedSlotNumber = 0;
    public LinkedList<Item2> SelectedSlot => ItemSlots[SelectedSlotNumber];
    public GameObject SelectedSlotUi => GameObject.Find(GetSlotName(SelectedSlotNumber));

    //Player global funds and money stuff
    private int _money = 100;

    private bool hasCreatedUi = false;
    // ToDo: Move player inventory elements from player data to here. static?

    // // Array of linked lists, each indice contains an item slot
    // public LinkedList<Item2>[] itemSlots;
    //
    // // A pointer to the slot the player has control over
    // public LinkedList<Item2> selectedSlot;
    // public int selectedSlotNumber;
    // public GameObject selectedSlotUI;
    //
    // //Player global funds and money stuff
    // public int money = 100;
    public bool inBinRange = false;

    public GameObject shovelPrefab;

    public Sprite startingInventorySelection;

    void Start()
    {
        ItemSlots = CreateItemSlots(SlotCount);
        // create slot UI in scene
        CreateSlotUi(SlotCount);

        // Add shovel from start
        GameObject shovel = Instantiate(shovelPrefab, Vector2.zero, Quaternion.identity);
        AddItem(shovel.GetComponent<Item2>());
        SelectedSlotUi.transform.GetComponent<Image>().sprite = startingInventorySelection;
    }

    private void CreateSlotUi(int slotCount)
    {
        // if (hasCreatedUi) return;
        var inventory = GameObject.Find(InventoryGameObjectName);
        foreach (Transform child in inventory.transform)
        {
            Destroy(child.gameObject);
        }

        var posX = PosX;
        var slot = (GameObject) Resources.Load(SlotUiPrefabPath);

        for (var i = 0; i < slotCount; i++)
        {
            var go = Instantiate(slot, new Vector3(posX, DefaultPosY, .0f), Quaternion.identity);
            if (go == null) throw new NullReferenceException();
            go.name = GetSlotName(i);
            go.transform.SetParent(inventory.transform);
            posX += DefaultSlotWidth;
        }

        // hasCreatedUi = true;
    }

    private static string GetSlotName(int slotNumber)
    {
        return "Slot" + (slotNumber + 1) + "UI";
    }

    private LinkedList<Item2>[] CreateItemSlots(int slotCount)
    {
        // if (hasCreatedUi) return ItemSlots;

        var slots = new LinkedList<Item2>[slotCount];
        for (var i = 0; i < slotCount; i++)
        {
            slots[i] = new LinkedList<Item2>();
        }

        // hasCreatedUi = true;
        return slots;
    }

    void Update()
    {
    }

    public bool AddItem(Item2 item)
    {
        // slotToAdd will remain -1 until end only if inventory is full
        var slotToAdd = -1;

        // Either find the lowest slot number, or the slot number that's item matches the item if it is stackable
        for (var i = 0; i < ItemSlots.Length; i++)
        {
            if (slotToAdd == -1 && ItemSlots[i].Count == 0)
                //But keep looking, as there may be a stackable slot later on
                slotToAdd = i;

            if (!item.is_stackable) continue;
            if (ItemSlots[i].Count <= 0 || item.itemName != ItemSlots[i].First.Value.itemName) continue;
            slotToAdd = i;
            //No matter what add it to the slot of existing items. No need to look further
            break;
        }

        if (slotToAdd == -1) return false;

        ItemSlots[slotToAdd].AddFirst(item);
        item.transform.gameObject.GetComponent<SpriteRenderer>().sprite = item.InventorySprite;
        item.transform.position = new Vector3(-500, 0, 0);
        // Update inventory GUI on screen
        UpdateUi(GetSlotName(slotToAdd), item, slotToAdd);
        return true;
    }

    private void UpdateUi(string slotName, Item2 item, int slotNumber)
    {
        var slotUi = GameObject.Find(slotName).transform;
        var component = slotUi.GetChild(0).GetComponent<Image>();
        var spriteRenderer = item.gameObject.GetComponent<SpriteRenderer>();

        var itemColor = spriteRenderer.color;
        component.sprite = spriteRenderer.sprite;
        // Check if item is being added to active slot. Adjust color appropriately.
        component.color = SelectedSlotNumber == slotNumber
            ? new Color(itemColor.r, itemColor.g, itemColor.b, 0.5f)
            : itemColor;

        if (item.is_stackable)
            slotUi.GetChild(1).GetComponent<TextMeshProUGUI>().text = "x " + ItemSlots[slotNumber].Count;
        item.transform.position = new Vector3(-500, 0, 0);
    }

    public bool DropItem()
    {
        var selectedSlot = ItemSlots[SelectedSlotNumber];
        Debug.Log("Attempting to drop");
        if (selectedSlot.Count <= 0) return false;
        // Do not allow player to drop their shovel
        if (selectedSlot.First.Value != null && selectedSlot.First.Value.itemName == "Shovel")
            return false;

        Item2 droppedItem = selectedSlot.First.Value;
        droppedItem.GetComponent<SpriteRenderer>().enabled = true;
        selectedSlot.Remove(droppedItem);
        if (selectedSlot.Count == 0)
        {
            // For shorter reference
            Image image = SelectedSlotUi.transform.GetChild(0).GetComponent<Image>();
            // Remove visibility of item icon by setting alpha to 0
            image.color = new Color(image.color.r, image.color.g, image.color.b, 0f);
            // clear text
            SelectedSlotUi.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = "";
        }
        else
        {
            SelectedSlotUi.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text =
                "" + ItemSlots[SelectedSlotNumber].Count;
        }

        return true;
    }

    public void UseSelectedItem(Vector2 location)
    {
        if (SelectedSlot.Count != 0)
        {
            Item2 item = SelectedSlot.First.Value;
            Vector3Int tileCoord = WorldData2.p1DiggableLayer.WorldToCell(location); //Get cell location
            // if item is a seed it adds a tile based on the editor
            if (item.is_seed)
            {
                if ((WorldData2.p1DiggableLayer.GetTile(tileCoord) == null &&
                     WorldData2.p2DiggableLayer.GetTile(tileCoord) == null) &&
                    (WorldData2.plantableLayer.GetTile(tileCoord) != null) &&
                    (WorldData2.CheckPlantedLocation(tileCoord)))
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
        Item2 item = SelectedSlot.First.Value;
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
        if (SelectedSlot.First.Value.is_consumable)
        {
            Item2 consumedItem = SelectedSlot.First.Value;
            SelectedSlot.Remove(consumedItem);
            //Destroy(consumedItem);
            if (SelectedSlot.Count == 0)
            {
                Image image = SelectedSlotUi.transform.GetChild(0).GetComponent<Image>(); // For shorter reference
                image.sprite = null;
                image.color = new Color(1, 1, 1, 0.5f);
                SelectedSlotUi.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = ""; // clear text
            }
            else
                SelectedSlotUi.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text =
                    "x " + ItemSlots[SelectedSlotNumber].Count;
        }
    }

    public void AddMoney(int value)
    {
        if (_money + value > 0)
        {
            _money += value;
        }
        else
        {
            _money = 0;
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
        bearTrap.GetComponent<BearTrap>().trapOwnerTag = userTag;
        if (userTag == PlayerData2.localPlayer.tag)
            ItemUsed();
    }
}
