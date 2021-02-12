using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Panel_HostJoin : MonoBehaviour
{
    [SerializeField] public GameObject panel_nameInput;
    [SerializeField] public GameObject panel_host;
    [SerializeField] public GameObject panel_join;

    public void Button_Host()
    {
        gameObject.SetActive(false);
        panel_host.SetActive(true);
    }

    public void Button_Join()
    {
        gameObject.SetActive(false);
        panel_join.SetActive(true);
    }

    public void Button_Cancel()
    {
        // go back
        gameObject.SetActive(false);
        panel_nameInput.SetActive(true);
    }
}
