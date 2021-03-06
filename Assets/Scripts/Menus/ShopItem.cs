﻿using System;
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

            if (playerMoveUpgrade)
            {
                PlayerData.AddMoney(-cost);
                PlayerMoveUpgrade.Activate();
                Destroy(this.gameObject);
                SoundControl.PlayPurchaseSound();
            }

            if (plantGrowthUpgrade)
            {
                PlayerData.AddMoney(-cost);
                PlantGrowthUpgrade.Activate();
                Destroy(this.gameObject);
                SoundControl.PlayPurchaseSound();
            }

            if (roundPoint)
            {
                //Only charge if an order was succesfully removed
                if (RoundPoint.Activate())
                {
                    PlayerData.AddMoney(-cost);
                    SoundControl.PlayPurchaseSound();
                }
            }

            if (plowField)
            {
                PlayerData.AddMoney(-cost);
                PlayerData2.localPlayer.GetComponent<PlowField>().Activate();
                SoundControl.PlayPurchaseSound();
                Destroy(this.gameObject);
            }

            if (bearTrap)
            {
                PlayerData.AddMoney(-cost);
                SoundControl.PlayPurchaseSound();
                GameObject bearTrapItemClone = Instantiate(ObjectData.bearTrapItemPrefab, Vector2.zero, Quaternion.identity);
                PlayerData2.localPlayer.GetComponent<PlayerInventory2>().AddItem(bearTrapItemClone.GetComponent<Item2>());
                GameObject bearTrapItemClone1 = Instantiate(ObjectData.bearTrapItemPrefab, Vector2.zero, Quaternion.identity);
                PlayerData2.localPlayer.GetComponent<PlayerInventory2>().AddItem(bearTrapItemClone1.GetComponent<Item2>());
            }

        }
        GameObject.Find("Shop").GetComponent<ShopSystem>().UpdateText();
    }
}
