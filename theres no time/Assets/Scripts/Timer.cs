using UnityEngine;
using UnityEngine.UI;
using System;
using TMPro;

public class Timer : MonoBehaviour
{
    // timer variables
    public TextMeshProUGUI timerText;
    public float timeRemaining = 180;
    private bool hasMoved = false;
    private GameObject player;

    private void Start()
    {
        player = GameObject.Find("Player");
    }

    void Update()
    {
        // double check to start the timer if the player moves
        if (!hasMoved && player.GetComponent<PlayerMovement>().hasMoved)
        {
            hasMoved = true;
        }

        // if the player has moved, decrease the timer
        if (hasMoved)
        {
            if (timeRemaining > 0)
            {
                timeRemaining -= Time.deltaTime;
                DisplayTime(timeRemaining);
            }
            else
            {
                timeRemaining = 0;
                hasMoved = false;
            }
        }
    }

    // display the current time in the textmeshpro boy :3
    void DisplayTime(float timeToDisplay)
    {
        float minutes = Mathf.FloorToInt(timeToDisplay / 60);
        float seconds = Mathf.FloorToInt(timeToDisplay % 60);
        float milliseconds = (timeToDisplay % 1) * 1000;

        milliseconds = Mathf.Round(milliseconds);

        timerText.text = string.Format("{0:00}:{1:00}:{2:00}", minutes, seconds, Mathf.FloorToInt(milliseconds / 10));
    }
}