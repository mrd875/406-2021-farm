using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.Tilemaps;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Mirror;

public class Item2 : NetworkBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public string itemName;
    public Tile actionTile;
    public GameObject actionPrefab;
    public Sprite InventorySprite;

    [HideInInspector]
    public bool pickup_allowed = false;
    public bool is_stackable = true;
    public bool is_consumable = true;
    public bool is_seed = false;
    
    private bool canSwap;


    public void OnPointerEnter(PointerEventData eventData)
    {
        PlayerData2.localPlayer.GetComponent<PlayerClick>().highlightedInteractable = gameObject;
    }


    public void OnPointerExit(PointerEventData eventData)
    {
        PlayerData2.localPlayer.GetComponent<PlayerClick>().highlightedInteractable.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1);
        PlayerData2.localPlayer.GetComponent<PlayerClick>().highlightedInteractable = null;
    }


    //Allows item pickup if player is in range
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // If player one walks over item
        if (collision.CompareTag("Player"))
        {
            pickup_allowed = true;
            Debug.Log("pick me up");
        }
    }
    
    
    //Does not allow item pickup if player is not in range
    private void OnTriggerExit2D(Collider2D collision)
    {
        // If player one moves away from item
        if (collision.CompareTag("Player"))
        {
            pickup_allowed = false;
        }
    }

    /*
    //Changes to the items inventory sprite
    public void ChangeToInventorySprite()
    {
        this.GetComponent<SpriteRenderer>().sprite = InventorySprite;
    }

    //Adds item to inventory
    private void AddItem()
    {
        Debug.Log("Attempting to add item");
        Sprite oldSprite = this.GetComponent<SpriteRenderer>().sprite;
        ChangeToInventorySprite();
        // attempt to add item. Canswap takes in a bool returned by AddItem indicating its success
        //canSwap = .AddItem(this);

        if (!canSwap)
        {
            this.GetComponent<SpriteRenderer>().sprite = oldSprite;
        }
    }
    */

}
