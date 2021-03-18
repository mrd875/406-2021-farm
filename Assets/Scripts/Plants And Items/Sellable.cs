using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sellable : MonoBehaviour
{
    // public int sellPrice;
    public OrderSystem orders;
    private Item2 inventoryInfo;
    public int sellPrice;

    private string itemName;

    void Start()
    {
        inventoryInfo = this.GetComponent<Item2>();
        orders = FindObjectOfType<OrderSystem>();
        itemName = inventoryInfo.itemName.Substring(9);
    }

    public bool SellPlant()
    {
        PlayerData.AddMoney(sellPrice);
        (bool, int) orderCheck = orders.CheckTickets(itemName);

        if (orderCheck.Item1)
        {
            orders.UpdateTicket(itemName, orderCheck.Item2);
            return true;
        }
        else {
            return true;
        }
    }
}
