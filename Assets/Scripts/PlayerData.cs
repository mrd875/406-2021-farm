using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerData : MonoBehaviour
{
    static public GameObject player;

    static public LinkedList<Item>[] itemSlots;
    static public LinkedList<Item> selectedSlot;
    static public int selectedSlotNumber;

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
    }

    // Update is called once per frame
    void Update()
    {
        selectedSlot = itemSlots[selectedSlotNumber];
    }

    static public bool EquipItem(Item item)
    {
        if ((itemSlots[0].Count == 0) || (item.is_stackable && item.itemName == itemSlots[0].First.Value.itemName))
        {
            itemSlots[0].AddFirst(item);
            item.transform.position = new Vector3(0, 0, 0);
            item.GetComponent<SpriteRenderer>().enabled = false;;
            return true;
        }
        else if ((itemSlots[1].Count == 0) || (item.is_stackable && item.itemName == itemSlots[1].First.Value.itemName))
        {
            itemSlots[1].AddFirst(item);
            item.transform.position = new Vector3(0, 0, 0);
            item.GetComponent<SpriteRenderer>().enabled = false; ;
            return true;
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

        }
    }

    static public void ItemUsed()
    {
        if (selectedSlot.First.Value.is_consumable)
        {
            Item consumedItem = selectedSlot.First.Value;
            selectedSlot.Remove(consumedItem);
            Destroy(consumedItem);
        }
    }
}
