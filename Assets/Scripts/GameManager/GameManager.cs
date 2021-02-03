using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{

    public void StartGame()
    {
        SceneManager.LoadScene("InventoryTestScene");
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
