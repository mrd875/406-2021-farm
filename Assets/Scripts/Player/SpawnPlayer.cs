using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPlayer : MonoBehaviour
{

    public GameObject player;
    public GameObject playersCamera;
    
    //Spawns a player on start
    void Start()
    {
        GameObject newPlayer = Instantiate(player, this.transform.position, this.transform.localRotation);

        PlayerData.SetPlayer(newPlayer);
    }

}
