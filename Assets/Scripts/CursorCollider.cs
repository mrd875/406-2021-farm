using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorCollider : MonoBehaviour
{
    void OnTriggerStay2D(Collider2D other)
    {
        if (other.tag == "Cursor")
        {
            PlayerData2.playerClick.canPlaceItem = false;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "Cursor")
        {
            PlayerData2.playerClick.canPlaceItem = true;
        }
    }
}
