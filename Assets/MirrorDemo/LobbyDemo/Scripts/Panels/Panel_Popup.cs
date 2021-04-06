using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class Panel_Popup : MonoBehaviour
{
    [SerializeField] public TMP_Text status;
    [SerializeField] public GameObject other_panel;

    Scene currentScene;

    void Start()
    {
        currentScene = SceneManager.GetActiveScene();
    }

    public void DoPopup(string message)
    {
        status.text = message;
        other_panel.SetActive(false);
        gameObject.SetActive(true);
    }

    public void Button_Close()
    {
        if (currentScene.name == "Scene_Lobby")
        {
            gameObject.SetActive(false);
            other_panel.SetActive(true);
        }
        else
        {
            SceneManager.LoadScene("Scene_Lobby");
        }
    }
}
