using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SellingBin : MonoBehaviour
{
    private GameObject enteredPlayer;

    // Bool to check if the player is in the radius of the bin
    public bool hasEntered = false;


    private void OnMouseDown()
    {
        Debug.Log("Clicked");
        if (hasEntered)
        {
            //PlayerData.AddItem(seed);
            enteredPlayer.GetComponent<PlayerInventory2>().SellItem();
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
