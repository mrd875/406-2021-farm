using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using TMPro;
using System;

public class Chat : NetworkBehaviour
{
    [SerializeField] private GameObject chatUI = null;
    [SerializeField] private TMP_Text chatText = null;
    [SerializeField] private TMP_InputField inputField = null;

    private static event Action<string> OnMessage;

    public override void OnStartAuthority()
    {
        // only enable our own chat ui objects, leaving the other player's chat ui disabled
        chatUI.SetActive(true);

        // subscribe to new messages from the server
        OnMessage += HandleNewMessage;
    }

    [ClientCallback]

    private void OnDestroy()
    {
        // unsubscribe to incoming server messages when our object gets destroied
        if (!hasAuthority) return;

        OnMessage -= HandleNewMessage;
    }

    private void HandleNewMessage(string message)
    {
        // append the server's chat message to our chat ui text
        chatText.text += message;
    }


    [Client]
    public void Send(string message)
    {
        // clients can send a chat message to the server

        // check if the return key is pressed
        if (!Input.GetKeyDown(KeyCode.Return)) return;

        // check if message is valid
        if (string.IsNullOrWhiteSpace(message)) return;

        //send the chat message to the server!
        CmdSendMessage(message);

        // clear the input
        inputField.text = string.Empty;
    }

    [Command]
    private void CmdSendMessage(string message)
    {
        //server will receive a client's chat message to send

        // validate chat message

        // send the chat message to the clients
        RpcHandleMessage($"[{connectionToClient.connectionId}]: {message}");
    }

    [ClientRpc]
    private void RpcHandleMessage(string message)
    {
        // clients will execute this when they recieve a chat message from the server

        // invoke the subscribers that we recieved a message
        OnMessage?.Invoke($"\n{message}");
    }
}
