using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using TMPro;

// Static variables for player one
public class PlayerData : MonoBehaviour
{
    // Reference to player object
    static public GameObject playerOne;
    static public Rigidbody2D playerOneRb;
    static public CircleCollider2D interactionRadius;

    static public GameObject localPlayer;
    static public PolygonCollider2D userArea;

    // Array of linked lists, each indice contains an item slot
    static public LinkedList<Item>[] itemSlots;

    // A pointer to the slot the player has control over
    static public LinkedList<Item> selectedSlot;
    static public int selectedSlotNumber;
    static public GameObject selectedSlotUI;

    // Players stamina value : not yet used for anything
    static public float maxStamina = 100;
    static public float currentStamina = 100;

    //Player global funds and money stuff
    static public int money = 100;
    static public bool inBinRange = false;
    
    // Bool for when user is in home zone trigger collider
    static public bool inHomeZone = false;



    // Start is called before the first frame update
    void Start()
    {
        playerOne = GameObject.Find("Player One");
        playerOneRb = playerOne.GetComponent<Rigidbody2D>();
        interactionRadius = GameObject.Find("PlayerOneInteractionRadius").GetComponent<CircleCollider2D>();

        localPlayer = playerOne;
        userArea = WorldData.playerOneZone;

        itemSlots = new LinkedList<Item>[5];
        itemSlots[0] = new LinkedList<Item>();
        itemSlots[1] = new LinkedList<Item>();
        itemSlots[2] = new LinkedList<Item>();
        itemSlots[3] = new LinkedList<Item>();
        itemSlots[4] = new LinkedList<Item>();

        selectedSlotNumber = 0;
        selectedSlotUI = GameObject.Find("Slot1UI");

        CreatePlayer(playerOne.transform.parent, .0f, .0f);
    }

    /// <summary>
    /// create player GameObject on demand
    /// </summary>
    /// <param name="parent"></param>
    /// <param name="x"></param>
    /// <param name="y"></param>
    private static void CreatePlayer(Transform parent, float x, float y)
    {
        GameObject go =
            Instantiate(Resources.Load("Player"), new Vector3(x, y, .0f), Quaternion.identity) as GameObject;
        // TODO: set player's position dynamically
        go.transform.parent = parent;
    }

    void Update()
    {
        selectedSlot = itemSlots[selectedSlotNumber];
    }

    // Adds item either to a slot already containing the same item type, or to a new slot
    static public bool AddItem(Item item)
    {
        int slotToAdd = -1; // slotToAdd will remain -1 until end only if inventory is full

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
            item.transform.position = new Vector3(-500, 0, 0);
        }

        // Update inventory GUI on screen
        switch (slotToAdd)
        {
            case 0:
                GameObject.Find("Slot1UI").transform.GetChild(0).GetComponent<Image>().sprite = item.gameObject.GetComponent<SpriteRenderer>().sprite;
                GameObject.Find("Slot1UI").transform.GetChild(0).GetComponent<Image>().color = item.gameObject.GetComponent<SpriteRenderer>().color;
                if (item.is_stackable)
                    GameObject.Find("Slot1UI").transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = "" + itemSlots[0].Count;
                return true;

            case 1:
                GameObject.Find("Slot2UI").transform.GetChild(0).GetComponent<Image>().sprite = item.gameObject.GetComponent<SpriteRenderer>().sprite;
                GameObject.Find("Slot2UI").transform.GetChild(0).GetComponent<Image>().color = item.gameObject.GetComponent<SpriteRenderer>().color;
                if (item.is_stackable)
                    GameObject.Find("Slot2UI").transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = "" + itemSlots[1].Count;
                return true;

            case 2:
                GameObject.Find("Slot3UI").transform.GetChild(0).GetComponent<Image>().sprite = item.gameObject.GetComponent<SpriteRenderer>().sprite;
                GameObject.Find("Slot3UI").transform.GetChild(0).GetComponent<Image>().color = item.gameObject.GetComponent<SpriteRenderer>().color;
                if (item.is_stackable)
                    GameObject.Find("Slot3UI").transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = "" + itemSlots[2].Count;
                return true;

            case 3:
                GameObject.Find("Slot4UI").transform.GetChild(0).GetComponent<Image>().sprite = item.gameObject.GetComponent<SpriteRenderer>().sprite;
                GameObject.Find("Slot4UI").transform.GetChild(0).GetComponent<Image>().color = item.gameObject.GetComponent<SpriteRenderer>().color;
                if (item.is_stackable)
                    GameObject.Find("Slot4UI").transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = "" + itemSlots[3].Count;
                return true;
            
            case 4:
                GameObject.Find("Slot5UI").transform.GetChild(0).GetComponent<Image>().sprite = item.gameObject.GetComponent<SpriteRenderer>().sprite;
                GameObject.Find("Slot5UI").transform.GetChild(0).GetComponent<Image>().color = item.gameObject.GetComponent<SpriteRenderer>().color;
                if (item.is_stackable)
                    GameObject.Find("Slot5UI").transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = "" + itemSlots[4].Count;
                return true;
            case -1:
                // inventory full
                return false; 
         
        }
        return false;
    }

    // drops item at player location
    static public void DropItem()
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
                Image image = selectedSlotUI.transform.GetChild(0).GetComponent<Image>();   // For shorter reference
                image.color = new Color(image.color.r, image.color.g, image.color.b, 0f);   // Remove visibility of item icon by setting alpha to 0
                selectedSlotUI.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = ""; // clear text
            }
            else
                selectedSlotUI.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = "" + itemSlots[selectedSlotNumber].Count;
        }
    }

    // calls function stored in held item's script
    static public void UseSelectedItem(Vector2 location)
    {
        if (selectedSlot.First.Value.UseItem(location))
        {
            ItemUsed(); // should be called after an item is successfully used
        }
    }

    // Deals with item consumption, durability loss, etc.
    static public void ItemUsed()
    {
        // Consumption
        if (selectedSlot.First.Value.is_consumable)
        {
            Item consumedItem = selectedSlot.First.Value;
            selectedSlot.Remove(consumedItem);
            Destroy(consumedItem);
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


    //Adjust player funds. Can be passed a negative value. Safeguards against going under 0
    static public void AddMoney(int value)
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
}
