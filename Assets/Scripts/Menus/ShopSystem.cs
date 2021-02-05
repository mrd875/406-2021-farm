using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopSystem : MonoBehaviour
{
    private bool hasEntered = false;

    public GameObject shopWindow;

    public int totalMoney = 500;
    public Text moneyText;

    void Start() {
        UpdateText();
    }

    private void Update() {
        if(hasEntered && Input.GetKeyDown(KeyCode.P)) {
            if(shopWindow.activeInHierarchy) {
                shopWindow.SetActive(false);
            }
            else {
                shopWindow.SetActive(true);
            }
        }
    }

    public void UpdateText() {
        moneyText.text = "$" + totalMoney + ".00";
    }

    private void OnTriggerEnter2D(Collider2D collider) {
        if(collider.tag == "PlayerOne") {
            hasEntered = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other) {
        shopWindow.SetActive(false);
        hasEntered = false;
    }
}
