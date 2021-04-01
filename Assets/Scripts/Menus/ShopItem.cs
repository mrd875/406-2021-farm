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
    public bool bearTrap;

    public GameObject itemPrefab;


    // Update player's total money count
    public void PurchaseItem() {
        if (PlayerData.money >= cost)
        {
            SoundControl.PlayPurchaseSound();
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
                PlayerData2.localPlayer.GetComponent<PlowField>().Activate();
            }

            if (bearTrap)
            {
                GameObject bearTrapItemClone = Instantiate(ObjectData.bearTrapItemPrefab, Vector2.zero, Quaternion.identity);
                PlayerData2.localPlayer.GetComponent<PlayerInventory2>().AddItem(bearTrapItemClone.GetComponent<Item2>());
                GameObject bearTrapItemClone1 = Instantiate(ObjectData.bearTrapItemPrefab, Vector2.zero, Quaternion.identity);
                PlayerData2.localPlayer.GetComponent<PlayerInventory2>().AddItem(bearTrapItemClone1.GetComponent<Item2>());
            }

        }
        GameObject.Find("Shop").GetComponent<ShopSystem>().UpdateText();
    }
}
