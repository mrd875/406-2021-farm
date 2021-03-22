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
    public bool roundPoint;
    public bool plowField;

    public GameObject itemPrefab;


    // Update player's total money count
    public void PurchaseItem() {
        if (PlayerData.money >= cost)
        {
            PlayerData.AddMoney(-cost);
            if (playerMoveUpgrade)
            {
                PlayerMoveUpgrade.Activate();
                Destroy(this.gameObject);
            }

            if (plantGrowthUpgrade)
            {
                PlantGrowthUpgrade.Activate();
                Destroy(this.gameObject);
            }

            if (roundPoint)
            {
                RoundPoint.Activate();
            }

            if (plowField)
            {
                GetComponent<PlowField>().Activate();
            }  
        }
        GameObject.Find("Shop").GetComponent<ShopSystem>().UpdateText();
    }
}
