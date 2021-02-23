using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Follow : MonoBehaviour
{
    void Start()
    {
        gameObject.transform.position = new Vector3(PlayerData.playerOne.transform.position.x,
            PlayerData.playerOne.transform.position.y, this.transform.position.z);
    }

    void Update()
    {
        gameObject.transform.position = new Vector3(PlayerData.playerOne.transform.position.x,
            PlayerData.playerOne.transform.position.y, this.transform.position.z);
    }
}
