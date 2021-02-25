using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopItem : MonoBehaviour
{
    public int cost = 25;
    // Update player's total money count
    public void PurchaseItem() {
        if (PlayerData.money >= cost)
        {
            PlayerData.AddMoney(-cost);
        }

        GameObject.Find("Shop").GetComponent<ShopSystem>().UpdateText();
    }
}
