﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopItem : MonoBehaviour
{
    // Update player's total money count
    public void PurchaseItem() {
        GameObject.Find("Shop").GetComponent<ShopSystem>().totalMoney = 
        GameObject.Find("Shop").GetComponent<ShopSystem>().totalMoney - 25;

        GameObject.Find("Shop").GetComponent<ShopSystem>().UpdateText();
    }
}
