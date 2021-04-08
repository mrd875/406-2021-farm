using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class RadicalProgressBar : MonoBehaviour
{
    public GameObject LoadingBar;

    void Update()
    {
        if (PlayerData2.localPlayer == null)
            return;
        LoadingBar.GetComponent<Image>().fillAmount = PlayerData2.playerShoot.cooldownProgress / PlayerData2.playerShoot.shotCooldown;
    }
}
