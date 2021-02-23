using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Panel_Popup : MonoBehaviour
{
    [SerializeField] public TMP_Text status;

    public void DoPopup(string message)
    {
        status.text = message;
        gameObject.SetActive(true);
    }

    public void Button_Close()
    {
        gameObject.SetActive(false);
    }
}
