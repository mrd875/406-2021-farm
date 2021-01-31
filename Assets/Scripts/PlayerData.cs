using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class PlayerData : MonoBehaviour
{
    // Reference to player object
    static public GameObject player;

    // Array of linked lists, each indice contains an item slot
    static public LinkedList<Item>[] itemSlots;

    // A pointer to the slot the player has control over
    static public LinkedList<Item> selectedSlot;
    static public int selectedSlotNumber;
    static public GameObject selectedSlotUI;

    // Players stamina value
    static public float maxStamina = 100;
    static public float currentStamina = 100;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player");
        itemSlots = new LinkedList<Item>[5];
        itemSlots[0] = new LinkedList<Item>();
        itemSlots[1] = new LinkedList<Item>();
        itemSlots[2] = new LinkedList<Item>();
        itemSlots[3] = new LinkedList<Item>();
        itemSlots[4] = new LinkedList<Item>();

        selectedSlotNumber = 0;
        selectedSlotUI = GameObject.Find("Slot1UI");
    }

    // Update is called once per frame
    void Update()
    {
        selectedSlot = itemSlots[selectedSlotNumber];
    }

    // Adds item either to a slot already containing the same item type, or to a new slot
    static public bool AddItem(Item item)
    {
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
            item.transform.position = new Vector3(0, 0, 0);
            item.GetComponent<SpriteRenderer>().enabled = false;
        }
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

    static public void DropItem()
    {
        Debug.Log("Attempting to drop");
        if (selectedSlot.Count > 0)
        {
            Item droppedItem = selectedSlot.First.Value;
            droppedItem.GetComponent<SpriteRenderer>().enabled = true;
            droppedItem.transform.position = player.transform.position;
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

    // 
    static public void UseSelectedItem()
    {
        if (selectedSlot.First.Value.UseItem())
        {
            ItemUsed();
        }
    }

    // Deals with item consumption, durability loss, etc.
    static public void ItemUsed()
    {
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
}
