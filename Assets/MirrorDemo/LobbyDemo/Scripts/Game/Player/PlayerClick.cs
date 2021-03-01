using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;
using Mirror;

using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;

public class PlayerClick : NetworkBehaviour
{
    private Tilemap gl;

    // Inventory script belonging to this.gameObject
    private PlayerInventory2 inventory;

    private int oldSlotNumber = 0;
    private string[] slotNames = new string[] { "Slot1UI", "Slot2UI", "Slot3UI", "Slot4UI", "Slot5UI" };

    private void Start()
    {
        gl = GameObject.FindGameObjectWithTag("GameGrid").GetComponent<Tilemap>();
        inventory = gameObject.GetComponent<PlayerInventory2>();
    }

    private void Update()
    {
        // only listen for clicks on our own game object.
        if (!hasAuthority)
            return;

        //Scroll to change items
        if (Input.mouseScrollDelta.y > 0)
        {
            if (oldSlotNumber < 4)
            {
                SetSlot(slotNames[oldSlotNumber + 1], oldSlotNumber + 1);
            }
            else
            {
                SetSlot(slotNames[0], 0);
            }
        }

        if (Input.mouseScrollDelta.y < 0)
        {
            if (oldSlotNumber > 0)
            {
                SetSlot(slotNames[oldSlotNumber - 1], oldSlotNumber - 1);
            }
            else
            {
                SetSlot(slotNames[4], 4);
            }
        }

        // Change Item Cursor with keys 1-5
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            SetSlot("Slot1UI", 0);
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            SetSlot("Slot2UI", 1);
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            SetSlot("Slot3UI", 2);
        }
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            SetSlot("Slot4UI", 3);
        }
        if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            SetSlot("Slot5UI", 4);
        }

        // Drop Item
        if (Input.GetKeyDown(KeyCode.R))
        {
            CmdDropItem(inventory.selectedSlot.First.Value.GetComponent<Item2>(), gameObject.transform.position);
            inventory.DropItem();
        }

        if (Input.GetMouseButtonDown(0))
        {
            Vector2 mousePos = Input.mousePosition;
            Vector2 worldPosition2D = Camera.main.ScreenToWorldPoint(mousePos);
            Vector3 worldPosition = new Vector3(worldPosition2D.x, worldPosition2D.y, this.transform.position.z);
            RaycastHit2D hit = Physics2D.Raycast(worldPosition2D, Vector2.zero);

            if (hit.collider != null && hit.collider.gameObject.GetComponent<Item2>() != null)
            {
                inventory.AddItem(hit.collider.gameObject.GetComponent<Item2>());
                CmdAddItem(hit.collider.gameObject.GetComponent<Item2>());
            }
            else if (inventory.selectedSlot.First.Value != null)
            {
                inventory.UseSelectedItem(worldPosition);
            }
            else
            {
                Debug.Log("false");
            }

        }

        if (Input.GetMouseButtonDown(1))
        {
            // get the position of the click

            Vector2 mousePos = Input.mousePosition;
            Vector2 worldPosition2D = Camera.main.ScreenToWorldPoint(mousePos);
            Vector3 worldPosition = new Vector3(worldPosition2D.x, worldPosition2D.y, transform.position.z);
            Vector3Int worldPos = gl.WorldToCell(worldPosition);

            // locally update our tile
            gl.SetTile(worldPos, null);

            // tell the server to tell other clients about our click
            CmdSetTile(worldPos);
        }
    }

    //Changes to the given slot. slotName must match exactly to the scene name of UI element.
    public void SetSlot(string slotName, int slotNumber)
    {
        //There is something in the slot you are leaving. Restore opacity
        if (inventory.itemSlots[oldSlotNumber].Count != 0)
        {
            inventory.selectedSlotUI.transform.GetChild(0).GetComponent<Image>().color = new Color(1, 1, 1, 1);
        }
        //Leaving an empty slot. Make it invisible
        else
        {
            inventory.selectedSlotUI.transform.GetChild(0).GetComponent<Image>().color = new Color(1, 1, 1, 0);
        }
        //Store data to check on next slot change
        oldSlotNumber = slotNumber;

        //Change to new item
        inventory.selectedSlotNumber = slotNumber;
        inventory.selectedSlotUI = GameObject.Find(slotName);
        inventory.selectedSlotUI.transform.GetChild(0).GetComponent<Image>().color = new Color(1, 1, 1, 0.5f);
    }

    [Command]
    private void CmdSetTile(Vector3Int v)
    {
        // tell other clients about our click
        RpcSetTile(v);
    }

    [Command]
    private void CmdAddItem(Item2 i)
    {
        RpcAddItem(i);
    }

    [Command]
    private void CmdDropItem(Item2 i, Vector2 v)
    {
        RpcDropItem(i, v);
    }

    [ClientRpc]
    private void RpcSetTile(Vector3Int v)
    {
        // update our tile
        gl.SetTile(v, null);
    }

    [ClientRpc]
    private void RpcAddItem(Item2 i)
    {
        i.transform.position = new Vector3(-500, 0, 0);
    }

    [ClientRpc]
    private void RpcDropItem(Item2 i, Vector2 v)
    {
        i.transform.position = v;
    }
}
