using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMoveUpgrade : MonoBehaviour
{
    public static void Activate()
    {
        PlayerMovement2 playerMoveScript = PlayerData2.localPlayer.GetComponent<PlayerMovement2>();
        playerMoveScript.SpeedUpgrade();
        //playerMoveScript.moveSpeed = playerMoveScript.moveSpeed * 1.4f;
    }

}
