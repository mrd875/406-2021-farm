using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerZone : MonoBehaviour
{
    // The tag of the player that this zone belongs to
    public string zoneOwnerTag = "PlayerOne"; // default value

    void OnTriggerStay2D(Collider2D other)
    {
        if (other.tag == "PlayerOne" || other.tag == "PlayerTwo" || other.tag == "PlayerThree" || other.tag == "PlayerFour")
        {
            if (other.tag == zoneOwnerTag)
            {
                // allow balloon throw
                // allow certain tool uses
                other.gameObject.GetComponent<PlayerInteraction>().inHomeZone = true;
            }
            else
            {

            }
        }
    }
    void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "PlayerOne" || other.tag == "PlayerTwo" || other.tag == "PlayerThree" || other.tag == "PlayerFour")
        {
            if (other.tag == zoneOwnerTag)
            {
                other.gameObject.GetComponent<PlayerInteraction>().inHomeZone = false;
                Debug.Log(zoneOwnerTag + " left their zone");
            }
            else
            {

            }
        }
    }
}
