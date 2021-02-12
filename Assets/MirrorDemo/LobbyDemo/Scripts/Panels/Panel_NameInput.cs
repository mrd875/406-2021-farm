using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Panel_NameInput : MonoBehaviour
{
    [SerializeField] public TMP_InputField inputName;
    [SerializeField] public TMP_Text statusText;
    [SerializeField] public GameObject panel_hostJoin;


    public void OnEnable()
    {
        statusText.text = "";
        inputName.text = gameObject.GetComponentInParent<Lobby_UI>().GetDefaultName();
    }

    public void Button_Confirm()
    {
        var name = inputName.text;

        // set display name...

        // validate our name is good...
        statusText.text = "Setting our name...";
        gameObject.GetComponentInParent<Lobby_UI>().SetName(name);
    }

    public void OnNameSetOutcome(bool success, string reason = "")
    {
        if (success)
        {
            // go onto the next menu
            gameObject.SetActive(false);
            panel_hostJoin.SetActive(true);
            return;
        }

        statusText.text = reason;
    }

    public void Button_Cancel()
    {
        // do nothing as this is the first menu for now
        statusText.text = "This IS the main menu for now...";
    }
}
