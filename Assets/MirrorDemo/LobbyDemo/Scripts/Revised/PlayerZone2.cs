using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerZone2 : MonoBehaviour
{
    // The tag of the player that this zone belongs to
    public string zoneOwnerTag = "PlayerOne"; // default value

    void OnTriggerStay2D(Collider2D other)
    {
        if (other.tag == "PlayerOne" || other.tag == "PlayerTwo")
        {
            if (other.tag != zoneOwnerTag)
            {
                if (other.gameObject.GetComponent<PlayerTouch>().inHomeZone)
                    Debug.Log(other.tag + " left their zone");
                other.gameObject.GetComponent<PlayerTouch>().inHomeZone = false;
            }
        }
    }
    void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "PlayerOne" || other.tag == "PlayerTwo")
        {
            if (other.tag != zoneOwnerTag)
            {
                other.gameObject.GetComponent<PlayerTouch>().inHomeZone = true;
                
            }
        }
    }
}
