using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lobby_UI : MonoBehaviour
{
    [SerializeField] private Panel_Hosting hostingPanel;
    [SerializeField] private Panel_Joining joiningPanel;
    [SerializeField] private Panel_Lobby lobbyPanel;
    [SerializeField] private Panel_NameInput nameSetPanel;

    private const string PlayerPrefsNameKey = "PlayerName";

    public void StartHosting(short port)
    {
        hostingPanel.OnHostOutcome(true);
    }

    public void CancelHosting()
    {

    }

    public void StartJoining(string ip, short port)
    {
        joiningPanel.OnJoinOutcome(true);
    }

    public void CancelJoining()
    {

    }

    public void SetName(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            nameSetPanel.OnNameSetOutcome(false, "<color=red>Name cannot be empty!</color>");
            return;
        }

        if (name.Length < 3)
        {
            nameSetPanel.OnNameSetOutcome(false, "<color=red>Name must be at least 3 chars long!</color>");
            return;
        }

        PlayerPrefs.SetString(PlayerPrefsNameKey, name);
        nameSetPanel.OnNameSetOutcome(true);
    }

    public void Go_Pressed()
    {

    }

    public void Button_Kick(int num)
    {

    }

    public void LeaveLobby()
    {
        lobbyPanel.OnLeftLobby();
    }

    public bool IsReady()
    {
        return false;
    }

    public int MinPlayers()
    {
        return 2;
    }

    public bool IsHost()
    {
        return true;
    }

    public int GetLobbyPlayerNum()
    {
        return 0;
    }

    public List<string> GetLobbyPlayerNames()
    {
        return new List<string>();
    }

    public List<bool> GetLobbyPlayerReadyStates()
    {
        return new List<bool>();
    }

    public string GetDefaultName()
    {
        if (!PlayerPrefs.HasKey(PlayerPrefsNameKey)) return "";

        return PlayerPrefs.GetString(PlayerPrefsNameKey);
    }
}
