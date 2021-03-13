using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SeedBin : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public Item2 seed;
    private GameObject enteredPlayer;

    // Bool to check if the player is in the radius of the bin
    public bool hasEntered = false;


    public void OnPointerEnter(PointerEventData eventData)
    {
        PlayerData2.localPlayer.GetComponent<PlayerClick>().highlightedInteractable = gameObject;
    }


    public void OnPointerExit(PointerEventData eventData)
    {
        PlayerData2.localPlayer.GetComponent<PlayerClick>().highlightedInteractable.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1);
        PlayerData2.localPlayer.GetComponent<PlayerClick>().highlightedInteractable = null;
    }


    private void OnMouseDown()
    {
        if (hasEntered)
        {
            //PlayerData.AddItem(seed);
            enteredPlayer.GetComponent<PlayerInventory2>().AddItem(seed);
        }
    }
    private void OnTriggerEnter2D(Collider2D collider)
    {
        enteredPlayer = collider.transform.gameObject;
        hasEntered = true;
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        hasEntered = false;
    }


}
