using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoundPoint : MonoBehaviour
{

    public static void Activate()
    {
        GameObject localplayer = PlayerData2.localPlayer.gameObject;
        localplayer.GetComponent<PlayerScore>().UpdateScore();
    }
}
