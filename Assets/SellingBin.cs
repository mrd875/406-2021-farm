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
        if (collider.tag == "Player")
        {
            enteredPlayer = collider.transform.gameObject;
            hasEntered = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            hasEntered = false;
        }
    }

}
