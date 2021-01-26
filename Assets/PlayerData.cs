using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerData : MonoBehaviour
{
    static public GameObject player;

    static public GameObject itemSlot1;
    static public GameObject itemSlot2;
    static public GameObject itemSlot3;
    static public GameObject itemSlot4;
    static public GameObject itemSlot5;

    static public LinkedList<DroppedItem> itemStack;

    static public float maxStamina = 100;
    static public float currentStamina = 100;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player");
        itemStack = new LinkedList<DroppedItem>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    static public bool EquipItem(DroppedItem item)
    {
        if ((itemStack.Count == 0) || (item.is_stackable && item.itemName == itemStack.First.Value.itemName))
        {
            itemStack.AddFirst(item);
            item.transform.position = new Vector3(0, 0, 0);
            item.GetComponent<SpriteRenderer>().enabled = false;;
            return true;
        }

        return false;
    }

    static public void DropItem()
    {
        Debug.Log("Attempting to drop");
        if (itemStack.Count > 0)
        {
            DroppedItem droppedItem = itemStack.First.Value;
            droppedItem.GetComponent<SpriteRenderer>().enabled = true;
            droppedItem.transform.position = player.transform.position;
            itemStack.Remove(droppedItem);

        }
    }

    static public void ItemUsed()
    {
        if (itemStack.First.Value.is_consumable)
        {
            DroppedItem consumedItem = itemStack.First.Value;
            itemStack.Remove(consumedItem);
            Destroy(consumedItem);
        }
    }
}
