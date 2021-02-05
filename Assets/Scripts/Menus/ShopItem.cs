using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopItem : MonoBehaviour
{
    public void PurchaseItem() {
        GameObject.Find("ShopWindow").GetComponent<ShopSystem>().totalMoney = 
        GameObject.Find("ShopWindow").GetComponent<ShopSystem>().totalMoney - 25;

        GameObject.Find("ShopWindow").GetComponent<ShopSystem>().UpdateText();
    }
}
