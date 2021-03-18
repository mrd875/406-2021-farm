using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// Static variables for player one
public class PlayerData : MonoBehaviour
{
    const int POS_X = 30;
    const int DEFAULT_SLOT_WIDTH = 105;

    const int DEFAULT_POS_Y = -40;

    // Reference to player object
    static public GameObject playerOne;
    static public Rigidbody2D playerOneRb;
    static public CircleCollider2D interactionRadius;

    static public GameObject localPlayer; // the player being controlled
    static public PolygonCollider2D userArea; // localPlayer's field collider area

    // Array of linked lists, each indice contains an item slot
    static public LinkedList<Item>[] itemSlots;

    // A pointer to the slot the player has control over
    static public LinkedList<Item> selectedSlot;
    static public int selectedSlotNumber;
    static public GameObject selectedSlotUI;

    // Reference to an item clicked on
    static public Item itemClicked;

    // Players stamina value : not yet used for anything
    // static public float maxStamina = 100;
    // static public float currentStamina = 100;

    //Player global funds and money stuff
    static public int money = 100;
    static public bool inBinRange = false;

    // Bool for when user is in home zone trigger collider
    // static public bool inHomeZone = false;


    // Start is called before the first frame update
    void Start()
    {
        playerOne = GameObject.Find("Player One");
        playerOneRb = playerOne.GetComponent<Rigidbody2D>();
        interactionRadius = GameObject.Find("PlayerOneInteractionRadius").GetComponent<CircleCollider2D>();

        localPlayer = playerOne;
        userArea = WorldData.playerOneZone;

        const int len = 5;
        itemSlots = CreateItemSlots(len);
        CreateSlotUI(len);

        selectedSlotNumber = 0;
        selectedSlotUI = GameObject.Find("Slot1UI");
    }

    private void CreateSlotUI(int slotCount)
    {
        var inventory = GameObject.Find("Inventory");
        var posX = POS_X;
        var slot = (GameObject) Resources.Load("Prefabs/SlotUI");

        for (var i = 0; i < slotCount; i++)
        {
            var go = Instantiate(slot, new Vector3(posX, DEFAULT_POS_Y, .0f), Quaternion.identity);
            if (go == null) throw new NullReferenceException();
            // go.transform.position = new Vector3(0, 0, .0f);
            go.name = "Slot" + (i + 1) + "UI";
            go.transform.parent = inventory.transform;
            posX += DEFAULT_SLOT_WIDTH;
        }
    }

    private static LinkedList<Item>[] CreateItemSlots(int slotCount)
    {
        var slots = new LinkedList<Item>[slotCount];
        for (var i = 0; i < slotCount; i++)
        {
            slots[i] = new LinkedList<Item>();
        }

        return slots;
    }

    private static string GetSlotName(int slotNumber)
    {
        switch (slotNumber)
        {
            case 0:
                return "Slot1UI";
            case 1:
                return "Slot2UI";
            case 2:
                return "Slot3UI";
            case 3:
                return "Slot4UI";
            case 4:
                return "Slot5UI";
            default:
                return null;
        }
    }

    void Update()
    {
        selectedSlot = itemSlots[selectedSlotNumber];
    }

    // Adds item either to a slot already containing the same item type, or to a new slot
    public static void AddItem(Item item)
    {
        int slotToAdd = -1; // slotToAdd will remain -1 until end only if inventory is full

        // Either find the lowest slot number, or the slot number thats item matches the item if it is stackable
        for (var i = 0; i < itemSlots.Length; i++)
        {
            if (slotToAdd == -1 && itemSlots[i].Count == 0)
                slotToAdd = i;
            if (item.is_stackable)
            {
                if (itemSlots[i].Count > 0 && item.itemName.Equals(itemSlots[i].First.Value.itemName))
                    slotToAdd = i;
            }
            else if (slotToAdd != -1)
            {
                break;
            }
        }

        if (slotToAdd == -1) return;
        itemSlots[slotToAdd].AddFirst(item);
        // Update inventory GUI on screen
        UpdateUI(GetSlotName(slotToAdd), item, slotToAdd);
    }

    private static void UpdateUI(string slotName, Item item, int slotNumber)
    {
        if (slotName == null)
        {
            return;
        }

        var slotObject = GameObject.Find(slotName);

        slotObject.transform.GetChild(0).GetComponent<Image>().sprite =
            item.gameObject.GetComponent<SpriteRenderer>().sprite;
        Debug.Log("Adding to slot " + slotNumber);
        Debug.Log("Active Slot: " + selectedSlotNumber);

        //Check if item is being added to active slot. Adjust color appropriately.
        if (selectedSlotNumber == slotNumber)
        {
            Debug.Log("Inside");
            Color itemColor = item.gameObject.GetComponent<SpriteRenderer>().color;
            slotObject.transform.GetChild(0).GetComponent<Image>().color =
                new Color(itemColor.r, itemColor.g, itemColor.b, 0.5f);
        }
        else
        {
            slotObject.transform.GetChild(0).GetComponent<Image>().color =
                item.gameObject.GetComponent<SpriteRenderer>().color;
        }

        if (item.is_stackable)
            slotObject.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text =
                "" + itemSlots[slotNumber].Count;
        WorldData.RemovePlantedLocation(WorldData.diggableLayer.WorldToCell(item.transform.localPosition));
        item.transform.position = new Vector3(-500, 0, 0);
    }

    // drops item at player location
    public static void DropItem()
    {
        Debug.Log("Attempting to drop");
        if (selectedSlot.Count > 0)
        {
            Item droppedItem = selectedSlot.First.Value;
            droppedItem.GetComponent<SpriteRenderer>().enabled = true;
            droppedItem.transform.position = playerOne.transform.position;
            selectedSlot.Remove(droppedItem);
            if (selectedSlot.Count == 0)
            {
                Image image = selectedSlotUI.transform.GetChild(0).GetComponent<Image>(); // For shorter reference
                image.color =
                    new Color(image.color.r, image.color.g, image.color.b,
                        0f); // Remove visibility of item icon by setting alpha to 0
                selectedSlotUI.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = ""; // clear text
            }
            else
                selectedSlotUI.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text =
                    "" + itemSlots[selectedSlotNumber].Count;
        }
    }

    // calls function stored in held item's script
    public static void UseSelectedItem(Vector2 location)
    {
        if (selectedSlot.Count != 0)
        {
            if (selectedSlot.First.Value.UseItem(location))
            {
                ItemUsed(); // should be called after an item is successfully used
            }
        }
    }

    // Deals with item consumption, durability loss, etc.
    public static void ItemUsed()
    {
        // Consumption
        if (selectedSlot.First.Value.is_consumable)
        {
            Item consumedItem = selectedSlot.First.Value;
            selectedSlot.Remove(consumedItem);
            Destroy(consumedItem);
            if (selectedSlot.Count == 0)
            {
                Image image = selectedSlotUI.transform.GetChild(0).GetComponent<Image>(); // For shorter reference
                image.sprite = null;
                image.color = new Color(1, 1, 1, 0.5f);
                selectedSlotUI.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = ""; // clear text
            }
            else
                selectedSlotUI.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text =
                    "" + itemSlots[selectedSlotNumber].Count;
        }
    }


    //Adjust player funds. Can be passed a negative value. Safeguards against going under 0
    public static void AddMoney(int value)
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
        UpdateMoney moneyText = FindObjectOfType<UpdateMoney>();
        moneyText.UpdateMoneyText();
    }

    public static void SetPlayer(GameObject newPlayer)
    {
        playerOne = newPlayer;
        playerOneRb = newPlayer.GetComponent<Rigidbody2D>();
        interactionRadius = newPlayer.transform.GetChild(0).GetComponent<CircleCollider2D>();
    }
}
