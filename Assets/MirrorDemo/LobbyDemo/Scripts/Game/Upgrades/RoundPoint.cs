using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoundPoint : MonoBehaviour
{

    public static bool Activate()
    {
        OrderSystem orderSystem = GameObject.FindObjectOfType<OrderSystem>();
        return orderSystem.OutsourceTicket();
    }
}
