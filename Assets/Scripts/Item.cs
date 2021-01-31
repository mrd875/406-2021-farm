using System.Collections;
using System.Collections.Generic;
using UnityEngine.Tilemaps;
using UnityEngine;

public class Item : MonoBehaviour
{
    public string itemName;
    public Tile actionTile;

    [HideInInspector]
    public bool pickup_allowed = false;
    public bool is_stackable = true;
    public bool is_consumable = true;
    public bool is_seed = false;

    private bool canSwap;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F) && pickup_allowed)
        {
            AddItem();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            pickup_allowed = true;
            Debug.Log("pick me up");
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        pickup_allowed = false;
    }

    private void AddItem()
    {
        canSwap = PlayerData.AddItem(this);
    }

    public bool UseItem()
    {
        if (is_seed)
        {
            Vector3Int pos = WorldData.topLayer.WorldToCell(PlayerData.player.transform.position);
            if (actionTile == null)
                Debug.Log("Error: No Item");
            if ((WorldData.topLayer.GetTile(pos) == null) && (WorldData.baseLayer.GetTile(pos) == WorldData.dirt))
            {
                WorldData.topLayer.SetTile(pos, actionTile);
                return true;
            }
        }
        else {
            switch (itemName)
            {
                case "Shovel":
                    Vector3Int pos = WorldData.topLayer.WorldToCell(PlayerData.player.transform.position);
                    if (WorldData.topLayer.GetTile(pos) != null)
                    {
                        WorldData.topLayer.SetTile(pos, null);
                        return true;
                    }
                    break;
            }
        }
        return false;
    }
}
