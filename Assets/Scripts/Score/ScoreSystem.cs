using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ScoreSystem : MonoBehaviour
{

    public int currentRound = 0;
    public int playerOneScore = 0;
    public int playerTwoScore = 0;
    
    public Text playerOneText;
    public Text playerTwoText;

    public float timeRemaining;
    public bool timerIsRunning = false;

    public Text timeText;

    public GameObject winScreen;

    private string playerOneName;
    private string playerTwoName;

    private string sceneName;

    // Start is called before the first frame update
    void Start()
    {
        NetworkRoomPlayerGame[] players = FindObjectsOfType<NetworkRoomPlayerGame>();

        foreach (var networkRoomPlayerGame in players)
        {
            if (networkRoomPlayerGame.index == 0)
            {
                playerOneName = networkRoomPlayerGame.displayName;
            } 
            else if (networkRoomPlayerGame.index == 1)
            {
                playerTwoName = networkRoomPlayerGame.displayName;
            }
        }

        // Starts the timer automatically
        timerIsRunning = true;
        sceneName = this.gameObject.scene.name;
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

    private void CalculateWinner()
    {

        PersistentRoundInfo thisRoundInfo = FindObjectOfType<PersistentRoundInfo>();

        //Game needs to go on to next round
        if (thisRoundInfo.currentRound < 3)
        {
            if (playerOneScore > playerTwoScore)
            {
                winScreen.GetComponent<Text>().color = new Color(0, 255, 255);
                winScreen.GetComponent<Text>().text = playerOneName + " Wins!";
                thisRoundInfo.playerOneScore += 1;
                thisRoundInfo.currentRound += 1;
            }
            else if (playerTwoScore > playerOneScore)
            {
                winScreen.GetComponent<Text>().color = new Color(0.92f, 0.5f, 0.04f, 1);
                winScreen.GetComponent<Text>().text = playerTwoName + " Wins!";
                thisRoundInfo.playerTwoScore += 1;
                thisRoundInfo.currentRound += 1;
            }
            else
            {
                winScreen.GetComponent<Text>().color = new Color(255, 255, 255);
                winScreen.GetComponent<Text>().text = "Draw!";
                thisRoundInfo.currentRound += 1;
            }
            SoundControl.PlayWinSound();
            winScreen.SetActive(true);
            StartCoroutine(NextRound());
        }
        //Game done
        else
        {
            if (playerOneScore > playerTwoScore)
            {
                FindObjectOfType<PersistentRoundInfo>().playerOneScore += 1;
            }
            else if (playerTwoScore > playerOneScore)
            {
                FindObjectOfType<PersistentRoundInfo>().playerTwoScore += 1;
            }

            if (thisRoundInfo.playerOneScore > thisRoundInfo.playerTwoScore)
            {
                winScreen.GetComponent<Text>().color = new Color(0,255,255);
                winScreen.GetComponent<Text>().text = "Game Over! \n" + playerOneName + " Wins!";
            }
            else if (thisRoundInfo.playerTwoScore > thisRoundInfo.playerOneScore)
            {
                winScreen.GetComponent<Text>().color = new Color(0.92f, 0.5f, 0.04f, 1);
                winScreen.GetComponent<Text>().text = "Game Over! \n" + playerTwoName + " Wins!";
            }

            else
            {
                winScreen.GetComponent<Text>().color = new Color(255, 255, 255);
                winScreen.GetComponent<Text>().text = "Game Over! \n Draw!";
            }

            SoundControl.PlayWinSound();
            winScreen.SetActive(true);
            StartCoroutine(EndGame());

        }
        
    }


    public void EndGameOveride()
    {
        //Should only really need to call on one player, relayed to others through server call. 
        PlayerAuthority playerOne = GameObject.FindWithTag("PlayerOne").GetComponent<PlayerAuthority>();
        playerOne.DisconnectPlayer();
    }


    private IEnumerator EndGame()
    {
        yield return new WaitForSeconds(4);
        Debug.Log("Game Done");
        //Should only really need to call on one player, relayed to others through server call. 
        PlayerAuthority playerOne = GameObject.FindWithTag("PlayerOne").GetComponent<PlayerAuthority>();
        playerOne.DisconnectPlayer();

    }

    private IEnumerator NextRound()
    {
        yield return new WaitForSeconds(4);
        Debug.Log("Next");
        RestartScene();
    }


    [Server]
    private void RestartScene()
    {
        NetworkManager thisManager = GameObject.FindObjectOfType<NetworkManager>();
        thisManager.ServerChangeScene(sceneName);
        Debug.Log("Switched Scene");
    }
}