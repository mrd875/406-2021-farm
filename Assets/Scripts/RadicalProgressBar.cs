using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class RadicalProgressBar : MonoBehaviour
{
    public GameObject LoadingBar;

    private float progress;
    /*public Transform TextIndicator;
    public Transform TextLoading;*/


    // Update is called once per frame
    void Update()
    {
        progress = PlayerData2.playerShoot.cooldownProgress / PlayerData2.playerShoot.shotCooldown;
        LoadingBar.GetComponent<Image>().fillAmount = progress;
    }
}
