using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreSystem : MonoBehaviour
{    
    public int playerOneScore = 0;
    public int playerTwoScore = 0;
    
    public Text playerOneText;
    public Text playerTwoText;

    void Update() {
        
        playerOneScore = GameObject.FindWithTag("PlayerOne").GetComponent<PlayerScore>().score;
        playerTwoScore = GameObject.FindWithTag("PlayerTwo").GetComponent<PlayerScore>().score;



        playerOneText.text = playerOneScore.ToString();
        playerTwoText.text = playerTwoScore.ToString();
    }
}
