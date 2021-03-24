using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ShopSystem : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    // String used to match against the collided player's tag
    public string playerTag;

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
        // local player isn't defined right away. This is to prevent errors
        if (PlayerData2.localPlayer == null)
            return;

        if (Vector2.Distance(PlayerData2.localPlayer.transform.position, gameObject.transform.position) < 3.0f)
            hasEntered = true;
        else
            hasEntered = false;

        // If player is out of range, make sure the shop is closed
        // This is in case that PlayerClick's interaction range is greater than radius of collider 
        if (PlayerData2.localPlayer.tag == playerTag && hasEntered == false)
        {
            shopWindow.SetActive(false);
        }
        // Check if the player is in the radius of the shop and has pressed the shop button
        /*        if(hasEntered && Input.GetKeyDown(KeyCode.P)) {
                    if(shopWindow.activeInHierarchy) {
                        shopWindow.SetActive(false);
                    }
                    else {
                        shopWindow.SetActive(true);
                    }
                }
                if(shopWindow.activeSelf) {
                    UpdateText();
                }*/
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        PlayerData2.playerClick.highlightedInteractable = gameObject;
    }


    public void OnPointerExit(PointerEventData eventData)
    {
        PlayerData2.playerClick.highlightedInteractable.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1);
        PlayerData2.playerClick.highlightedInteractable = null;
    }

    public void OpenShopWindow()
    {
        SoundControl.PlayShopSound();
        if (shopWindow.activeInHierarchy)
        {
            shopWindow.SetActive(false);
        }
        else
        {
            shopWindow.SetActive(true);
        }

        if (shopWindow.activeSelf)
        {
            UpdateText();
        }
    }

    // Update the text displaying the total money "$X.00"
    public void UpdateText() {
        moneyText.text = "$" + PlayerData.money + ".00";
    }

/*    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.tag == playerTag)
        {
            hasEntered = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        shopWindow.SetActive(false);
        hasEntered = false;
    }*/
}
