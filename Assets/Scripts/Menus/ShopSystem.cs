using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopSystem : MonoBehaviour
{
    public int totalMoney = 500;
    public Text moneyText;

    void Start() {
        UpdateText();
    }

    public void UpdateText() {
        moneyText.text = "$" + totalMoney + ".00";
    }

}
