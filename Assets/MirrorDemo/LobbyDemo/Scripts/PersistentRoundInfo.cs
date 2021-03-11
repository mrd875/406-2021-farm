using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PersistentRoundInfo : MonoBehaviour
{
    public int currentRound;

    public int playerOneScore;
    public int playerTwoScore;

    // Start is called before the first frame update
    void Start()
    {
        PersistentRoundInfo otherInfo = FindObjectOfType<PersistentRoundInfo>();

        if (otherInfo.gameObject != this.gameObject)
        {
            currentRound = otherInfo.currentRound;
            playerOneScore = otherInfo.playerOneScore;
            playerTwoScore = otherInfo.playerTwoScore;
            currentRound += 1;
            Destroy(otherInfo.gameObject);
        }
        else
        {
            currentRound = 1;
            playerOneScore = 0;
            playerTwoScore = 0;
        }
        DontDestroyOnLoad(this.gameObject);
    }

    void Update()
    {
        PersistentRoundInfo otherInfo = FindObjectOfType<PersistentRoundInfo>();
        if (otherInfo.gameObject != this.gameObject)
        {
            Debug.Log("We in the update now");
            if (otherInfo.currentRound > this.currentRound)
            {
                currentRound = otherInfo.currentRound;
            }
            else
            {
                currentRound += 1;
            }

            if (otherInfo.playerOneScore > this.playerOneScore)
            {
                playerOneScore = otherInfo.playerOneScore;
            }

            if (otherInfo.playerTwoScore > this.playerTwoScore)
            {
                playerTwoScore = otherInfo.playerTwoScore;
            }
            
            Destroy(otherInfo.gameObject);
        }

    }

}
