using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sellable : MonoBehaviour
{
    public int sellPrice;

    public void SellPlant()
    {
        PlayerData.AddMoney(sellPrice);

    }
}
