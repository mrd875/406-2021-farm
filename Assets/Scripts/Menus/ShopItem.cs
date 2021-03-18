using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEngine;

public class ShopItem : MonoBehaviour
{
    public int cost = 25;

    public bool playerMoveUpgrade;
    public bool plantGrowthUpgrade;


    // Update player's total money count
    public void PurchaseItem() {
        if (PlayerData.money >= cost)
        {
            PlayerData.AddMoney(-cost);
            if (playerMoveUpgrade)
            {
                PlayerMoveUpgrade.Activate();
            }

            if (plantGrowthUpgrade)
            {
                PlantGrowthUpgrade.Activate();
            }

            Destroy(this.gameObject);
        }
        


        
        GameObject.Find("Shop").GetComponent<ShopSystem>().UpdateText();

    }
}
