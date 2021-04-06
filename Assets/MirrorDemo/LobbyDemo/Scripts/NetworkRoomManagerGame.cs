using UnityEngine;
using Mirror;
using System;
using System.Collections.Generic;
using System.Linq;

/*
	Documentation: https://mirror-networking.com/docs/Components/NetworkRoomManager.html
	API Reference: https://mirror-networking.com/docs/api/Mirror.NetworkRoomManager.html

	See Also: NetworkManager
	Documentation: https://mirror-networking.com/docs/Components/NetworkManager.html
	API Reference: https://mirror-networking.com/docs/api/Mirror.NetworkManager.html
*/

/// <summary>
/// This is a specialized NetworkManager that includes a networked room.
/// The room has slots that track the joined players, and a maximum player count that is enforced.
/// It requires that the NetworkRoomPlayer component be on the room player objects.
/// NetworkRoomManager is derived from NetworkManager, and so it implements many of the virtual functions provided by the NetworkManager class.
/// </summary>
public class NetworkRoomManagerGame : NetworkRoomManager
{

    #region Server Callbacks

    /// <summary>
    /// This is called on the server when the server is started - including when a host is started.
    /// </summary>
    /// 
    public event Action OnRoomStartedServer;
    public override void OnRoomStartServer()
    {
        base.OnRoomStartServer();

        OnRoomStartedServer?.Invoke();
    }

    /// <summary>
    /// This is called on the server when the server is stopped - including when a host is stopped.
    /// </summary>
    public event Action OnRoomStoppedServer;
    public override void OnRoomStopServer()
    {
        base.OnRoomStopServer();

        OnRoomStoppedServer?.Invoke();
    }

    /// <summary>
    /// This is called on the host when a host is started.
    /// </summary>
    /// 
    public event Action OnRoomStartedHost;
    public override void OnRoomStartHost()
    {
        base.OnRoomStartHost();

        OnRoomStartedHost?.Invoke();
    }

    /// <summary>
    /// This is called on the host when the host is stopped.
    /// </summary>
    /// 
    public event Action OnRoomStoppedHost;
    public override void OnRoomStopHost()
    {
        base.OnRoomStopHost();

        OnRoomStoppedHost?.Invoke();
    }

    /// <summary>
    /// This is called on the server when a new client connects to the server.
    /// </summary>
    /// <param name="conn">The new connection.</param>
    /// 
    public event Action<NetworkConnection> OnRoomServerConnected;
    public override void OnRoomServerConnect(NetworkConnection conn)
    {
        base.OnRoomServerConnect(conn);

        OnRoomServerConnected?.Invoke(conn);
    }

    /// <summary>
    /// This is called on the server when a client disconnects.
    /// </summary>
    /// <param name="conn">The connection that disconnected.</param>
    /// 
    public event Action<NetworkConnection> OnRoomServerDisconnected;
    public override void OnRoomServerDisconnect(NetworkConnection conn)
    {
        base.OnRoomServerDisconnect(conn);

        OnRoomServerDisconnected?.Invoke(conn);
    }

    /// <summary>
    /// This is called on the server when a networked scene finishes loading.
    /// </summary>
    /// <param name="sceneName">Name of the new scene.</param>
    /// 
    public event Action<string> OnRoomServerChangedScene;
    public override void OnRoomServerSceneChanged(string sceneName)
    {
        base.OnRoomServerSceneChanged(sceneName);

        OnRoomServerChangedScene?.Invoke(sceneName);
    }

    /// <summary>
    /// This allows customization of the creation of the room-player object on the server.
    /// <para>By default the roomPlayerPrefab is used to create the room-player, but this function allows that behaviour to be customized.</para>
    /// </summary>
    /// <param name="conn">The connection the player object is for.</param>
    /// <returns>The new room-player object.</returns>
    /// 
    public event Action<NetworkConnection> OnRoomServerCreatedRoomPlayer;
    public override GameObject OnRoomServerCreateRoomPlayer(NetworkConnection conn)
    {
        OnRoomServerCreatedRoomPlayer?.Invoke(conn);

        return base.OnRoomServerCreateRoomPlayer(conn);
    }

    /// <summary>
    /// This allows customization of the creation of the GamePlayer object on the server.
    /// <para>By default the gamePlayerPrefab is used to create the game-player, but this function allows that behaviour to be customized. The object returned from the function will be used to replace the room-player on the connection.</para>
    /// </summary>
    /// <param name="conn">The connection the player object is for.</param>
    /// <param name="roomPlayer">The room player object for this connection.</param>
    /// <returns>A new GamePlayer object.</returns>
    /// 
    public event Action<NetworkConnection, GameObject> OnRoomServerCreatedGamePlayer;
    public override GameObject OnRoomServerCreateGamePlayer(NetworkConnection conn, GameObject roomPlayer)
    {
        OnRoomServerCreatedGamePlayer?.Invoke(conn, roomPlayer);

        GameObject gamePlayer = base.OnRoomServerCreateGamePlayer(conn, roomPlayer);
        return gamePlayer;
    }

    /// <summary>
    /// This allows customization of the creation of the GamePlayer object on the server.
    /// <para>This is only called for subsequent GamePlay scenes after the first one.</para>
    /// <para>See OnRoomServerCreateGamePlayer to customize the player object for the initial GamePlay scene.</para>
    /// </summary>
    /// <param name="conn">The connection the player object is for.</param>
    /// 
    public event Action<NetworkConnection> OnRoomServerAddedPlayer;
    public override void OnRoomServerAddPlayer(NetworkConnection conn)
    {
        base.OnRoomServerAddPlayer(conn);

        OnRoomServerAddedPlayer?.Invoke(conn);
    }

    /// <summary>
    /// This is called on the server when it is told that a client has finished switching from the room scene to a game player scene.
    /// <para>When switching from the room, the room-player is replaced with a game-player object. This callback function gives an opportunity to apply state from the room-player to the game-player object.</para>
    /// </summary>
    /// <param name="conn">The connection of the player</param>
    /// <param name="roomPlayer">The room player object.</param>
    /// <param name="gamePlayer">The game player object.</param>
    /// <returns>False to not allow this player to replace the room player.</returns>
    /// 
    public event Action<NetworkConnection, GameObject, GameObject> OnRoomServerSceneLoadedPlayer;
    public override bool OnRoomServerSceneLoadedForPlayer(NetworkConnection conn, GameObject roomPlayer, GameObject gamePlayer)
    {
        OnRoomServerSceneLoadedPlayer?.Invoke(conn, roomPlayer, gamePlayer);
        NetworkRoomPlayerGame nrpg = roomPlayer.GetComponent<NetworkRoomPlayerGame>();

        if (nrpg.index == 0)
            gamePlayer.tag = "PlayerOne";
        if (nrpg.index == 1)
            gamePlayer.tag = "PlayerTwo";

        return base.OnRoomServerSceneLoadedForPlayer(conn, roomPlayer, gamePlayer);
    }

    /// <summary>
    /// This is called on the server when all the players in the room are ready.
    /// <para>The default implementation of this function uses ServerChangeScene() to switch to the game player scene. By implementing this callback you can customize what happens when all the players in the room are ready, such as adding a countdown or a confirmation for a group leader.</para>
    /// </summary>
    /// 
    public event Action OnRoomServerPlayersReadied;
    public override void OnRoomServerPlayersReady()
    {
        // prevent default, we will handle this
        // base.OnRoomServerPlayersReady();

        OnRoomServerPlayersReadied?.Invoke();
    }

    /// <summary>
    /// This is called on the server when CheckReadyToBegin finds that players are not ready
    /// <para>May be called multiple times while not ready players are joining</para>
    /// </summary>
    /// 
    public event Action OnRoomServerPlayersNotReadied;
    public override void OnRoomServerPlayersNotReady()
    {
        base.OnRoomServerPlayersNotReady();

        OnRoomServerPlayersNotReadied?.Invoke();
    }

    #endregion

    #region Client Callbacks


    /// <summary>
    /// This is a hook to allow custom behaviour when the game client enters the room.
    /// </summary>
    /// 
    public event Action OnRoomClientEntered;
    public override void OnRoomClientEnter()
    {
        base.OnRoomClientEnter();

        OnRoomClientEntered?.Invoke();
    }

    /// <summary>
    /// This is a hook to allow custom behaviour when the game client exits the room.
    /// </summary>
    /// 
    public event Action OnRoomClientExited;
    public override void OnRoomClientExit()
    {
        base.OnRoomClientExit();

        OnRoomClientExited?.Invoke();
    }
     
    /// <summary>
    /// This is called on the client when it connects to server.
    /// </summary>
    /// <param name="conn">The connection that connected.</param>
    /// 
    public event Action<NetworkConnection> OnRoomClientConnected;
    public override void OnRoomClientConnect(NetworkConnection conn)
    {
        base.OnRoomClientConnect(conn);

        OnRoomClientConnected?.Invoke(conn);
    }

    /// <summary>
    /// This is called on the client when disconnected from a server.
    /// </summary>
    /// <param name="conn">The connection that disconnected.</param>
    /// 
    public event Action<NetworkConnection> OnRoomClientDisconnected;
    public override void OnRoomClientDisconnect(NetworkConnection conn)
    {
        base.OnRoomClientDisconnect(conn);

        OnRoomClientDisconnected?.Invoke(conn);
    }

    /// <summary>
    /// This is called on the client when a client is started.
    /// </summary>
    /// <param name="roomClient">The connection for the room.</param>
    /// 
    public event Action OnRoomStartedClient;
    public override void OnRoomStartClient()
    {
        base.OnRoomStartClient();

        OnRoomStartedClient?.Invoke();
    }

    /// <summary>
    /// This is called on the client when the client stops.
    /// </summary>
    /// 
    public event Action OnRoomStoppedClient;
    public override void OnRoomStopClient()
    {
        base.OnRoomStopClient();

        OnRoomStoppedClient?.Invoke();
    }

    /// <summary>
    /// This is called on the client when the client is finished loading a new networked scene.
    /// </summary>
    /// <param name="conn">The connection that finished loading a new networked scene.</param>
    /// 
    public event Action<NetworkConnection> OnRoomClientChangedScene;
    public override void OnRoomClientSceneChanged(NetworkConnection conn)
    {
        base.OnRoomClientSceneChanged(conn);

        OnRoomClientChangedScene?.Invoke(conn);
    }

    /// <summary>
    /// Called on the client when adding a player to the room fails.
    /// <para>This could be because the room is full, or the connection is not allowed to have more players.</para>
    /// </summary>
    /// 
    public event Action OnRoomClientAddPlayerFail;
    public override void OnRoomClientAddPlayerFailed()
    {
        base.OnRoomClientAddPlayerFailed();

        OnRoomClientAddPlayerFail?.Invoke();
    }

    #endregion

    #region Optional UI

    public override void OnGUI()
    {
        //base.OnGUI();
    }

    #endregion

}
