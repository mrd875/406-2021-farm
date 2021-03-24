using System.Collections;
using System.Collections.Generic;
using System.Net.Mime;
using Mirror;
using UnityEngine;
using UnityEngine.UI;

public class RoundNumber : MonoBehaviour
{
    public int currentRound;

    private Text myText;
    // Start is called before the first frame update
    void Start()
    {
        PersistentRoundInfo currentRoundHolder = FindObjectOfType<PersistentRoundInfo>();
        currentRound = currentRoundHolder.currentRound;

        myText = gameObject.GetComponent<Text>();
        myText.text = "Round " + currentRound.ToString();
    }

    void Update()
    {
        PersistentRoundInfo currentRoundHolder = FindObjectOfType<PersistentRoundInfo>();
        currentRound = currentRoundHolder.currentRound;

        myText.text = "Round " + currentRound.ToString();
    }







}
