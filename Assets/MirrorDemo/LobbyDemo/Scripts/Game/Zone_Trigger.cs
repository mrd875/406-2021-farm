using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Zone_Trigger : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
            collision.GetComponent<PlayerTouch>().inZone = transform.parent.gameObject;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            if (collision.GetComponent<PlayerTouch>().inZone == transform.parent.gameObject)
                collision.GetComponent<PlayerTouch>().inZone = null;
        }
    }
}
