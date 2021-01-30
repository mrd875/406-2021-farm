using System.Collections;
using System.Collections.Generic;
using UnityEngine.Tilemaps;
using UnityEngine;

public class Item : MonoBehaviour
{
    public string itemName;
    public Tile actionTile;

    public bool pickup_allowed = false;
    public bool is_stackable = true;
    public bool is_consumable = true;

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
        canSwap = PlayerData.EquipItem(this);
    }
}
