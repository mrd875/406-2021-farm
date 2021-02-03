using System.Collections;
using System.Collections.Generic;
using UnityEngine.Tilemaps;
using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    // Item use key
    public KeyCode itemKey = KeyCode.E;

    void Update()
    {
        // Change Item Cursor with keys 1-5
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            PlayerData.selectedSlotNumber = 0;
            PlayerData.selectedSlotUI = GameObject.Find("Slot1UI");
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            PlayerData.selectedSlotNumber = 1;
            PlayerData.selectedSlotUI = GameObject.Find("Slot2UI");
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            PlayerData.selectedSlotNumber = 2;
            PlayerData.selectedSlotUI = GameObject.Find("Slot3UI");
        }
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            PlayerData.selectedSlotNumber = 3;
            PlayerData.selectedSlotUI = GameObject.Find("Slot4UI");
        }
        if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            PlayerData.selectedSlotNumber = 4;
            PlayerData.selectedSlotUI = GameObject.Find("Slot5UI");
        }

        // Drop Item
        if (Input.GetKeyDown(KeyCode.R))
        {
            PlayerData.DropItem();
        }

        // Use Item
        if (Input.GetKeyDown(itemKey))
        {
            if (PlayerData.selectedSlot.Count > 0)
            {
                PlayerData.UseSelectedItem();
            }
        }
        
    }

}
