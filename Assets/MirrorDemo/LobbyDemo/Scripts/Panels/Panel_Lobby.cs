using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class Panel_Lobby : MonoBehaviour
{
    [SerializeField] public TMP_Text[] playerNameTexts;
    [SerializeField] public TMP_Text[] playerReadyTexts;
    [SerializeField] public GameObject[] kickButtons;
    [SerializeField] public TMP_Text status;
    [SerializeField] public TMP_Text btnTxt_Go;

    [SerializeField] public GameObject panel_hostJoin;

    public void OnEnable()
    {
        status.text = "In Lobby";
        OnUpdatePlayers();

        InvokeRepeating("OnUpdatePlayers", 0.3f, 0.3f);
    }

    public void OnDisable()
    {
        CancelInvoke("OnUpdatePlayers");
    }

    public void Button_Go()
    {
        // tell the game we pressed the go button
        gameObject.GetComponentInParent<Lobby_UI>().Go_Pressed();
    }

    public void Button_Leave()
    {
        // tell the game we want to leave
        gameObject.GetComponentInParent<Lobby_UI>().LeaveLobby();
    }

    public void OnLeftLobby()
    {
        // we left the lobby
        SceneManager.LoadScene("Scene_Lobby");
/*        gameObject.SetActive(false);
        panel_hostJoin.SetActive(true);*/
    }

    public void Button_Kick(int num)
    {
        gameObject.GetComponentInParent<Lobby_UI>().Button_Kick(num);
    }

    public void OnUpdatePlayers()
    {
        // ok we recieved a lobby update signal,
        // lets get all the data we need and update our stuff accordingly

        var names = gameObject.GetComponentInParent<Lobby_UI>().GetLobbyPlayerNames();
        var readies = gameObject.GetComponentInParent<Lobby_UI>().GetLobbyPlayerReadyStates();
        int minPlayers = gameObject.GetComponentInParent<Lobby_UI>().MinPlayers();
        bool isReady = gameObject.GetComponentInParent<Lobby_UI>().IsReady();
        bool isHost = gameObject.GetComponentInParent<Lobby_UI>().IsHost();
        int ourNum = gameObject.GetComponentInParent<Lobby_UI>().GetLobbyPlayerNum();

        // lets reset lobby state
        foreach (var name in playerNameTexts)
            name.text = "Waiting for player";
        foreach (var readyTxt in playerReadyTexts)
            readyTxt.text = "";
        foreach (var btn in kickButtons)
            btn.SetActive(false);

        // now update our view with the state we recieved
        // alot of assumptions here, names.Count == readies.Count,  readies.Count <= playerNameTexts.Length,   playerNameTexts.Length == playerReadyTexts.Length
        Debug.Assert(names.Count == readies.Count);
        Debug.Assert(readies.Count <= playerNameTexts.Length);
        Debug.Assert(playerNameTexts.Length == playerReadyTexts.Length);
        for (var i = 0; i < names.Count; i++)
        {
            playerNameTexts[i].text = names[i];

            playerReadyTexts[i].text = readies[i] ? 
                "<color=green>Ready</color>" :
                "<color=red>Not Ready</color>";

            if (i != ourNum)
                kickButtons[i].SetActive(isHost);
        }

        // check state of lobby
        bool allReady = true;
        foreach (var rdy in readies)
            if (!rdy)
                allReady = false;

        if (!allReady)
        {
            status.text = "Not everyone is ready";
        }

        if (readies.Count < minPlayers)
        {
            status.text = "We need more players";
        }

        btnTxt_Go.text = isReady ? "Unready Up" : "Ready Up";

        if (allReady && readies.Count >= minPlayers)
        {
            status.text = "Ready to start";

            if (isHost)
                btnTxt_Go.text = "Start Game";
        }
    }
}
