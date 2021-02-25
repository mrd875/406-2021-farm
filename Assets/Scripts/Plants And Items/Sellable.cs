using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sellable : MonoBehaviour
{
    public int sellPrice;
    public OrderSystem orders;
    private Item inventoryInfo;

    private string itemName;

    void Start()
    {
        inventoryInfo = this.GetComponent<Item>();
        orders = FindObjectOfType<OrderSystem>();
        itemName = inventoryInfo.itemName.Substring(9);
    }

    public void SellPlant()
    {
        PlayerData.localPlayer.GetComponent<PlayerInventory>().AddMoney(sellPrice);
        (bool, int) orderCheck = orders.CheckTickets(itemName);

        if (orderCheck.Item1)
        {
            orders.UpdateTicket(itemName, orderCheck.Item2);
        }
    }
}
