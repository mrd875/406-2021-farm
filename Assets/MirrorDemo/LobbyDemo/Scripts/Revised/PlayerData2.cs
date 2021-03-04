using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerData2 : MonoBehaviour
{
    static public GameObject playerOne;
    static public GameObject playerTwo;

    // Start is called before the first frame update
    void Start()
    {
        playerOne = GameObject.Find("GameObject_NetworkPlayer(Clone)");
        playerOne = GameObject.Find("GameObject_NetworkPlayer(Clone)(1)");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
