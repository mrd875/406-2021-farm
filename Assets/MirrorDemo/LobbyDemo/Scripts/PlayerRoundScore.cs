using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerRoundScore : MonoBehaviour
{
    public bool isPlayerOne;

    public int score;

    private Text myText;
    // Start is called before the first frame update
    void Start()
    {
        PersistentRoundInfo currentScoreHolder = FindObjectOfType<PersistentRoundInfo>();
        if (isPlayerOne)
        {
            score = currentScoreHolder.playerOneScore;
        }
        else
        {
            score = currentScoreHolder.playerTwoScore;
        }

        myText = gameObject.GetComponent<Text>();
        myText.text = score.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        PersistentRoundInfo currentScoreHolder = FindObjectOfType<PersistentRoundInfo>();
        if (isPlayerOne)
        {
            score = currentScoreHolder.playerOneScore;
        }
        else
        {
            score = currentScoreHolder.playerTwoScore;
        }

        myText.text = score.ToString();
    }
}
