using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class RadicalProgressBar : MonoBehaviour
{
    public GameObject LoadingBar;

    void Update()
    {
        LoadingBar.GetComponent<Image>().fillAmount = PlayerData2.playerShoot.cooldownProgress / PlayerData2.playerShoot.shotCooldown;
    }
}
