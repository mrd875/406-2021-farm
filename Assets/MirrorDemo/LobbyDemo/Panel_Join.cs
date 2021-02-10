using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Panel_Join : MonoBehaviour
{
    [SerializeField] private TMP_InputField portInput;
    [SerializeField] private TMP_InputField ipInput;
    [SerializeField] private GameObject panel_hostJoin;
    [SerializeField] private GameObject panel_joining;

    public void Button_Join()
    {
        var portStr = portInput.text;
        short port = short.Parse(portStr);

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
