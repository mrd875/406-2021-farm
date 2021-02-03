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
        SceneManager.LoadScene("InventoryTestScene");
    }

    // Exits the game
    public void QuitGame()
    {
        Application.Quit();
    }
}
