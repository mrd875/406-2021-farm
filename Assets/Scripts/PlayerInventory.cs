using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    // ToDo: Move player inventory elements from player data to here. static?

    // Array of linked lists, each indice contains an item slot
    static public LinkedList<Item>[] itemSlots;

    // Start is called before the first frame update
    void Start()
    {
        itemSlots = new LinkedList<Item>[5];
        itemSlots[0] = new LinkedList<Item>();
        itemSlots[1] = new LinkedList<Item>();
        itemSlots[2] = new LinkedList<Item>();
        itemSlots[3] = new LinkedList<Item>();
        itemSlots[4] = new LinkedList<Item>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
