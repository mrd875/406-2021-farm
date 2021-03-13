using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class PersistentRoundInfo : MonoBehaviour
{
    public int currentRound;

    public int playerOneScore;
    public int playerTwoScore;

    // Start is called before the first frame update
    void Start()
    {
        PersistentRoundInfo[] otherInfo = FindObjectsOfType<PersistentRoundInfo>();


        if (otherInfo.Length == 1)
        {
            //There is no one but me
                currentRound = 1;
                playerOneScore = 0;
                playerTwoScore = 0;
        }
        else
        {
            foreach (var persistentRoundInfo in otherInfo)
            {
                if (persistentRoundInfo.gameObject != this.gameObject)
                {

                    if (persistentRoundInfo.currentRound > this.currentRound)
                    {
                        Destroy(this.gameObject);
                    }

                }
            }
        }

        DontDestroyOnLoad(this.gameObject);
    }

    void Update()
    {
        PersistentRoundInfo[] otherInfo = FindObjectsOfType<PersistentRoundInfo>();
        
        foreach (var persistentRoundInfo in otherInfo)
        {
            if (persistentRoundInfo.gameObject != this.gameObject)
            {

                if (persistentRoundInfo.currentRound > this.currentRound)
                {
                    Destroy(this.gameObject);
                }

            }
        }
        

    }

}
