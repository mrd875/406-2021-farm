using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Panel_Join : MonoBehaviour
{
    [SerializeField] public TMP_InputField portInput;
    [SerializeField] public TMP_InputField ipInput;
    [SerializeField] public GameObject panel_hostJoin;
    [SerializeField] public GameObject panel_joining;

    public void OnEnable()
    {
        portInput.text = gameObject.GetComponentInParent<Lobby_UI>().GetDefaultPort();
        ipInput.text = gameObject.GetComponentInParent<Lobby_UI>().GetDefaultIP();
    }

    public void Button_Join()
    {
        var portStr = portInput.text;
        ushort port;

        try
        {
            port = ushort.Parse(portStr);
        }
        catch
        {
            Debug.LogError("Port was not valid");
            return;
        }

        var ip = ipInput.text;

        // go to hosting status menu
        gameObject.SetActive(false);
        panel_joining.SetActive(true);

        // init the hosting sequence
        gameObject.GetComponentInParent<Lobby_UI>().StartJoining(ip, port);
    }

    public void Button_Cancel()
    {
        // go back
        gameObject.SetActive(false);
        panel_hostJoin.SetActive(true);
    }
}
