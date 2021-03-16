using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// Controls scene switching
public class GameManager : MonoBehaviour
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
}
