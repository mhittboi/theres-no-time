using UnityEngine;
using UnityEngine.UI;
using System;
using TMPro;

public class Timer : MonoBehaviour
{

    // Timer Variables
    public TextMeshProUGUI timerText;
    public float timeRemaining = 180;
    private bool hasMoved = true;
    private int minutesDisplay;
    private int secondsDisplay;
    private int millisecondsDisplay;

    // Update is called once per frame
    void Update()
    {
        if (!hasMoved && GameObject.Find("Player").GetComponent<PlayerMovement>().hasMoved)
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
                Debug.Log("Time has run out.");
                timeRemaining = 0;
                hasMoved = false;
            }
        }
    }

    void DisplayTime(float timeToDisplay)
    {
        timeToDisplay += 1;
        float minutes = Mathf.FloorToInt(timeToDisplay / 60);
        float seconds = Mathf.FloorToInt(timeToDisplay % 60);
        float milliseconds = (timeToDisplay % 1) * 100;

        timerText.text = string.Format("{0:00}:{1:00}:{2:00}", minutes, seconds, milliseconds);
    }
}
