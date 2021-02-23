using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Panel_Host : MonoBehaviour
{
    [SerializeField] public TMP_InputField portInput;
    [SerializeField] public GameObject panel_hostJoin;
    [SerializeField] public GameObject panel_hosting;

    public void OnEnable()
    {
        portInput.text = gameObject.GetComponentInParent<Lobby_UI>().GetDefaultPort();
    }

    public void Button_Host()
    {
        var portStr = portInput.text;
        ushort port;

        try
        {
            port = ushort.Parse(portStr);
        }
        catch
        {
            Debug.LogError("Port was not valid.");
            return;
        }

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
