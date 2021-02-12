using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Panel_Joining : MonoBehaviour
{
    [SerializeField] public TMP_Text status;
    [SerializeField] public GameObject panel_join;
    [SerializeField] public GameObject panel_lobby;


    private bool flipDots;
    private int dots;
    public void OnEnable()
    {
        status.text = "Joining";
        flipDots = false;
        dots = 0;
        InvokeRepeating("Connect", 0.3f, 0.3f);
    }

    public void OnDisable()
    {
        CancelInvoke("Connect");
    }

    public void Connect()
    {
        if (!flipDots)
        {
            status.text += ".";
            dots++;

            if (dots > 2)
                flipDots = true;
        }
        else
        {
            status.text = status.text.Substring(0, status.text.Length - 1);
            dots--;

            if (dots <= 0)
                flipDots = false;
        }
    }

    public void OnJoinOutcome(bool success, string reason = "")
    {
        if (success)
        {
            // go to lobby
            gameObject.SetActive(false);
            panel_lobby.SetActive(true);
            return;
        }

        CancelInvoke("Connect");
        // tell the user the error
        status.text = reason;
    }

    public void Button_Cancel()
    {
        // cancel the hosting sequence if needed...
        gameObject.GetComponentInParent<Lobby_UI>().CancelJoining();

        // go back
        gameObject.SetActive(false);
        panel_join.SetActive(true);
    }
}
