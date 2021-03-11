using UnityEngine;
using UnityEngine.UI;

public class UpdateMoney : MonoBehaviour
{
    private Text moneyText;
    void Start()
    {
        moneyText = GetComponent<Text>();
        UpdateMoneyText();
    }


    public void UpdateMoneyText()
    {
        moneyText.text = "$" + PlayerData.money + ".00";
    }

}
