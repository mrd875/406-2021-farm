using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeedBin : MonoBehaviour
{

    public Item seed;

    // Bool to check if the player is in the radius of the bin
    public bool hasEntered = false;

    /*
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (hasEntered)
            {
                PlayerData.AddItem(seed);
            }
        }
    }
    */

    private void OnMouseDown()
    {
        Debug.Log("Clicked");
        if (hasEntered)
        {
            PlayerData.AddItem(seed);
            Debug.Log("Fired");
        }
    }
    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.tag == "PlayerOne")
        {
            hasEntered = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "PlayerOne")
        {
            hasEntered = false;
        }
    }


}
