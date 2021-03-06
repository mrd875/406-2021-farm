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

    public float timeRemaining;
    public bool timerIsRunning = false;

    public Text timeText;

    public GameObject winScreen;

    // Start is called before the first frame update
    void Start()
    {
        // Starts the timer automatically
        timerIsRunning = true;
    }


    void Update() {
        // Update scores
        playerOneScore = GameObject.FindWithTag("PlayerOne").GetComponent<PlayerScore>().score;
        playerTwoScore = GameObject.FindWithTag("PlayerTwo").GetComponent<PlayerScore>().score;

        // Update text
        playerOneText.text = playerOneScore.ToString();
        playerTwoText.text = playerTwoScore.ToString();

        if (timerIsRunning)
        {
            if (timeRemaining > 0)
            {
                timeRemaining -= Time.deltaTime;
            }
            else
            {
                Debug.Log("Time has run out!");
                timeRemaining = 0;
                timerIsRunning = false;

                CalculateWinner();
            }

            DisplayTime(timeRemaining);
        }
    }

    // Display time on screen "0:00" format
    private void DisplayTime(float timeToDisplay)
    {
        float minutes = Mathf.FloorToInt(timeToDisplay / 60);  
        float seconds = Mathf.FloorToInt(timeToDisplay % 60);

        timeText.text = string.Format("{0:0}:{1:00}", minutes, seconds);
    }

    private void CalculateWinner() {
        if(playerOneScore > playerTwoScore) {
            winScreen.GetComponent<Text>().text = "Player One Wins!";
            winScreen.SetActive(true);
        }
        else if(playerTwoScore > playerOneScore) {
            winScreen.GetComponent<Text>().text = "Player Two Wins!";
            winScreen.SetActive(true);
        }
        else {
            winScreen.GetComponent<Text>().text = "Draw!";
            winScreen.SetActive(true);
        }
    }


}
