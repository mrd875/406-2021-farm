using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Mirror;

// Controls scene switching
public class GameManager : NetworkBehaviour
{
    // Loads the game scene
    public void StartGame()
    {
        SceneManager.LoadScene("Scene_Lobby");
    }

    // Loads settings scene
    public void OpenSettings()
    {
        SceneManager.LoadScene("Settings");
    }

    // Loads menu
    public void OpenMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void OpenLobby()
    {
        SceneManager.LoadScene("Scene_Lobby");
    }

    // Exits the game
    public void QuitGame()
    {
        Application.Quit();
    }

    // When one of the players leaves the game.
    public void EndGame()
    {
        CmdEndGame();
    }

    public void MoveToScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    [Command(ignoreAuthority = true)]
    public void CmdEndGame()
    {
        RpcEndGame();
        NetworkManager.singleton.ServerChangeScene("Scene_Lobby");
    }

    [ClientRpc]
    private void RpcEndGame()
    {
        PlayerData2.localGrowSpeed = 1.0f;
        PlayerMovement2 playerMoveScript = PlayerData2.localPlayer.GetComponent<PlayerMovement2>();
        playerMoveScript.ResetSpeed();
        PlayerData.money = 100;
    }
}
