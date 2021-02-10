using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Panel_Host : MonoBehaviour
{
    [SerializeField] private TMP_InputField portInput;
    [SerializeField] private GameObject panel_hostJoin;
    [SerializeField] private GameObject panel_hosting;

    public void Button_Host()
    {
        var portStr = portInput.text;
        short port = short.Parse(portStr);

        // go to hosting status menu
        gameObject.SetActive(false);
        panel_hosting.SetActive(true);

        // init the hosting sequence
        gameObject.GetComponentInParent<Lobby_UI>().StartHosting(port);
    }

    public void Button_Cancel()
    {
        // go back
        gameObject.SetActive(false);
        panel_hostJoin.SetActive(true);
    }
}
