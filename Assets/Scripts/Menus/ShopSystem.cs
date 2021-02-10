using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopSystem : MonoBehaviour
{
    // Bool to check if the player is in the radius of the shop
    private bool hasEntered = false;

    // Shop Window to display
    public GameObject shopWindow;

    // Player's total money and the text to display that number
    private int totalMoney = PlayerData.money;
    public Text moneyText;

    void Start() {
        UpdateText();
    }

    private void Update() {
        // Check if the player is in the radius of the shop and has pressed the shop button
        if(hasEntered && Input.GetKeyDown(KeyCode.P)) {
            if(shopWindow.activeInHierarchy) {
                shopWindow.SetActive(false);
            }
            else {
                shopWindow.SetActive(true);
            }
        }
        if(shopWindow.activeSelf) {
            UpdateText();
        }
    }

    // Update the text displaying the total money "$X.00"
    public void UpdateText() {
        moneyText.text = "$" + PlayerData.money + ".00";
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
