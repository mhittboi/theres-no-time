using UnityEngine;
using UnityEngine.UI;
using System;
using TMPro;

public class Timer : MonoBehaviour
{
    // Timer Variables
    public TextMeshProUGUI timerText;
    public float timeRemaining = 180;
    private bool hasMoved = false;
    private int minutesDisplay;
    private int secondsDisplay;
    private int millisecondsDisplay;
    private GameObject player;

    private void Start()
    {
        player = GameObject.Find("Player"); // Find the player gameobject
    }

    // Update is called once per frame
    void Update()
    {
        if (!hasMoved && player.GetComponent<PlayerMovement>().hasMoved)
        {
            hasMoved = true;
        }

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

    void DisplayTime(float timeToDisplay)
    {
        float minutes = Mathf.FloorToInt(timeToDisplay / 60);
        float seconds = Mathf.FloorToInt(timeToDisplay % 60);
        float milliseconds = (timeToDisplay % 1) * 1000; // Changed here

        milliseconds = Mathf.Round(milliseconds);

        timerText.text = string.Format("{0:00}:{1:00}:{2:00}", minutes, seconds, Mathf.FloorToInt(milliseconds / 10));
    }
}