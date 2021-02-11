using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerZone : MonoBehaviour
{
    public string zoneOwnerTag = "PlayerOne";

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
                // become attackable
            }
        }
    }
    void OnTriggerLeave2D(Collider2D other)
    {
        if (other.tag == "PlayerOne" || other.tag == "PlayerTwo" || other.tag == "PlayerThree" || other.tag == "PlayerFour")
        {
            if (other.tag == zoneOwnerTag)
            {
                other.gameObject.GetComponent<PlayerInteraction>().inHomeZone = false;
                //other.gameObject.GetComponent<Shoot>().inHomeZone = false;
                //other.gameObject.GetComponent<PlayerInteraction>().inHomeZone = false;
            }
            else
            {
                // become attackable
            }
        }
    }
}
