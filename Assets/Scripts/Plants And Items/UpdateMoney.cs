using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class UpdateMoney : MonoBehaviour
{
    private Text moneyText;

    private GameObject gainSparkles;
    private GameObject loseSparkles;
    void Start()
    {
        moneyText = GetComponent<Text>();
        gainSparkles = transform.GetChild(0).gameObject;
        loseSparkles = transform.GetChild(1).gameObject;

        UpdateMoneyText();
    }


    public void UpdateMoneyText()
    {
        Debug.Log("Updating Money Text");
        moneyText.text = "$" + PlayerData.money + ".00";
    }

    public void PlayGain()
    {
        gainSparkles.GetComponent<UIAnim>().StartAnim();
    }

    public void PlayLoss()
    {
        loseSparkles.GetComponent<UIAnim>().StartAnim();
    }

}
