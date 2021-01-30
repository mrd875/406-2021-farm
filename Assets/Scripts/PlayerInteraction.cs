using System.Collections;
using System.Collections.Generic;
using UnityEngine.Tilemaps;
using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    public KeyCode cutterKey = KeyCode.Q;
    public KeyCode itemKey = KeyCode.E;
    public Tilemap grassMap;
    public Tilemap dirtMap;

    public Tile dirt;
    public Tile grass;
    //public Tile actionTile;


    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        // Change Item Cursor with keys 1-5
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            PlayerData.selectedSlotNumber = 0;
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            PlayerData.selectedSlotNumber = 1;
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            PlayerData.selectedSlotNumber = 2;
        }
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            PlayerData.selectedSlotNumber = 3;
        }
        if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            PlayerData.selectedSlotNumber = 4;
        }

        // Drop Item
        if (Input.GetKeyDown(KeyCode.R))
        {
            PlayerData.DropItem();
        }

        // Cut Tile
        if (Input.GetKeyDown(cutterKey))
        {
            Vector3Int pos = grassMap.WorldToCell(gameObject.transform.position);
            if (grassMap.GetTile(pos) != null)
                grassMap.SetTile(pos, null);
        }

        // Use Item
        if (Input.GetKeyDown(itemKey))
        {
            if (PlayerData.selectedSlot.Count > 0)
            {
                Vector3Int pos = grassMap.WorldToCell(gameObject.transform.position);
                if (PlayerData.selectedSlot.First.Value.actionTile == null)
                    Debug.Log("Error: No Item");
                if ((grassMap.GetTile(pos) == null) && (dirtMap.GetTile(pos) == dirt))
                {
                    grassMap.SetTile(pos, PlayerData.selectedSlot.First.Value.actionTile);
                    PlayerData.ItemUsed();
                }
            }
        }
        
    }

}
