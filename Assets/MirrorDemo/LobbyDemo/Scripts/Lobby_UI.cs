using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class Lobby_UI : MonoBehaviour
{
    [SerializeField] public Panel_Hosting hostingPanel;
    [SerializeField] public Panel_Joining joiningPanel;
    [SerializeField] public Panel_Lobby lobbyPanel;
    [SerializeField] public Panel_NameInput nameSetPanel;
    [SerializeField] public Panel_Popup popupPanel;

    private NetworkRoomManagerGame _netman;
    private NetworkRoomManagerGame NetMan
    {
        get
        {
            if (_netman != null) return _netman;

            _netman = NetworkManager.singleton as NetworkRoomManagerGame;
            if (_netman != null) return _netman;

            _netman = FindObjectOfType<NetworkRoomManagerGame>();
            return _netman;
        }
    }

    private NetworkRoomPlayerGame LocalPlayer
    {
        get
        {
            foreach (var player in NetMan.roomSlots)
            {
                if (player.hasAuthority)
                    return (NetworkRoomPlayerGame)player;
            }

            return null;
        }
    }

    private const string PlayerPrefsNameKey = "PlayerName";
    private const string PlayerPrefsPort = "GamePort";
    private const string PlayerPrefsIP = "GameIP";
    private string PerferedName;

    public static string popupMessageOnEnable = null;

    private void OnEnable()
    {
        // setup the callbacks
        NetMan.OnRoomClientConnected += OnRoomClientConnected;
        NetMan.OnRoomClientDisconnected += OnRoomClientDisconnected;

        NetMan.OnRoomClientEntered += OnRoomClientEntered;
        NetMan.OnRoomClientExited += OnRoomClientExited;


        // flush a popup message
        if (!string.IsNullOrWhiteSpace(popupMessageOnEnable))
        {
            popupPanel.DoPopup(popupMessageOnEnable);
            popupMessageOnEnable = null;
        }
    }

    private void OnDisable()
    {
        // clean up
        NetMan.OnRoomClientConnected -= OnRoomClientConnected;
        NetMan.OnRoomClientDisconnected -= OnRoomClientDisconnected;

        NetMan.OnRoomClientEntered -= OnRoomClientEntered;
        NetMan.OnRoomClientExited -= OnRoomClientExited;
    }

    // when anyone exits (even the local client)
    private void OnRoomClientExited()
    {
        // if it was us that left the lobby, lets tell the ui that we did
        if (LocalPlayer == null)
        {
            if (lobbyPanel.isActiveAndEnabled)
                lobbyPanel.OnLeftLobby();
        }
    }

    // when anyone enters (even the local client)
    private void OnRoomClientEntered()
    {
        
    }

    IEnumerator WaitForLocalPlayer()
    {
        while (LocalPlayer == null)
            yield return new WaitForSeconds(0.05f);

        // tell server what we want our name to be
        LocalPlayer.CmdChangeName(PerferedName);

        // update the ui of the successful outcome of connection
        if (hostingPanel.isActiveAndEnabled)
            hostingPanel.OnHostOutcome(true);
        else if (joiningPanel.isActiveAndEnabled)
            joiningPanel.OnJoinOutcome(true);
    }

    // when the local client connects
    private void OnRoomClientConnected(NetworkConnection obj)
    {
        StartCoroutine("WaitForLocalPlayer");
    }

    // when the local client disconnects
    private void OnRoomClientDisconnected(NetworkConnection obj)
    {
        // tell the ui that we disconnected
        if (joiningPanel.isActiveAndEnabled)
            joiningPanel.OnJoinOutcome(false, "Connection timed out.");
        else if (lobbyPanel.isActiveAndEnabled)
        {
            lobbyPanel.OnLeftLobby();

            popupMessageOnEnable = "Lost connection to lobby.";
            popupPanel.DoPopup(popupMessageOnEnable);
        }
    }

    public void StartHosting(ushort port)
    {
        PlayerPrefs.SetString(PlayerPrefsPort, port.ToString());
        NetMan.gameObject.GetComponent<kcp2k.KcpTransport>().Port = port;

        try
        {
            NetMan.StartHost();
        }
        catch (Exception e)
        {
            if (hostingPanel.isActiveAndEnabled)
                hostingPanel.OnHostOutcome(false, e.ToString());
        }
    }

    public void CancelHosting()
    {
        NetMan.StopHost();
    }

    public void StartJoining(string ip, ushort port)
    {
        PlayerPrefs.SetString(PlayerPrefsIP, ip);
        PlayerPrefs.SetString(PlayerPrefsPort, port.ToString());

        NetMan.networkAddress = ip;
        NetMan.gameObject.GetComponent<kcp2k.KcpTransport>().Port = port;

        try
        {
            NetMan.StartClient();
        }
        catch (Exception e)
        {
            joiningPanel.OnJoinOutcome(false, e.ToString());
        }
    }

    public void CancelJoining()
    {
        NetMan.StopClient();

        // stop transport from connecting... cause bugs when hosting before the timeout
    }

    public void Go_Pressed()
    {
        if (IsHost() && NetMan.allPlayersReady)
        {
            GameObject roundInfo = GameObject.Find("RoundInfo");
            if (roundInfo != null)
            {
                PersistentRoundInfo pri = roundInfo.GetComponent<PersistentRoundInfo>();
                pri.currentRound = 1;
                pri.playerOneScore = 0;
                pri.playerTwoScore = 0;
            }
            LocalPlayer.CmdStartGame();
/*            foreach (var player in NetMan.roomSlots)
            {
                player.CmdChangeReadyState(false);
            }*/

            return;
        }

        // toggle the ready state
        LocalPlayer.CmdChangeReadyState(!LocalPlayer.readyToBegin);
    }

    public void Button_Kick(int num)
    {
        LocalPlayer.CmdPlayerKickNum(num);
    }

    public void LeaveLobby()
    {
        if (IsHost())
            NetMan.StopHost();
        else
            NetMan.StopClient();

        lobbyPanel.OnLeftLobby();
    }

    public bool IsReady()
    {
        return LocalPlayer.readyToBegin;
    }

    public int MinPlayers()
    {
        return NetMan.minPlayers;
    }

    public bool IsHost()
    {
        return LocalPlayer.isHost;
    }

    public int GetLobbyPlayerNum()
    {
        for (var i = 0; i < NetMan.roomSlots.Count; i++)
        {
            if (NetMan.roomSlots[i].hasAuthority)
                return i;
        }

        return -1;
    }

    public List<string> GetLobbyPlayerNames()
    {
        var answer = new List<string>();

        foreach (var p in NetMan.roomSlots)
        {
            var player = (NetworkRoomPlayerGame)p;

            answer.Add(player.displayName);
        }

        return answer;
    }

    public List<bool> GetLobbyPlayerReadyStates()
    {
        var answer = new List<bool>();

        foreach (var p in NetMan.roomSlots)
        {
            var player = (NetworkRoomPlayerGame)p;

            answer.Add(player.readyToBegin);
        }

        return answer;
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

        if (name.Length > 20)
        {
            nameSetPanel.OnNameSetOutcome(false, "<color=red>Name cannot be longer than 20 chars!</color>");
            return;
        }

        PlayerPrefs.SetString(PlayerPrefsNameKey, name);
        PerferedName = name;
        nameSetPanel.OnNameSetOutcome(true);
    }

    public string GetDefaultName()
    {
        if (!PlayerPrefs.HasKey(PlayerPrefsNameKey)) return "Unknown Farmer";

        return PlayerPrefs.GetString(PlayerPrefsNameKey);
    }

    public string GetDefaultIP()
    {
        if (!PlayerPrefs.HasKey(PlayerPrefsIP)) return "localhost";

        return PlayerPrefs.GetString(PlayerPrefsIP);
    }

    public string GetDefaultPort()
    {
        if (!PlayerPrefs.HasKey(PlayerPrefsPort)) return "7777";

        return PlayerPrefs.GetString(PlayerPrefsPort);
    }
}
