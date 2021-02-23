using System.Collections;
using System.Collections.Generic;
using System.Net.Mime;
using UnityEngine;
using UnityEngine.UI;

public class UpdateMoney : MonoBehaviour
{
    private Text moneyText;
    void Start()
    {
        moneyText = this.GetComponent<Text>();
        UpdateMoneyText();
    }


    public void UpdateMoneyText()
    {
        moneyText.text = "$" + PlayerData.money.ToString();
    }

}
