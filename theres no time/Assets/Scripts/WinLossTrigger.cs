using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using TMPro;

public class WinLossTrigger : MonoBehaviour
{
    public GameObject victoryScreen; 
    public GameObject lossScreen;

    public List<Button> menuButtons;
    public Vector2 victoryLocation; 
    private int selectedButtonIndex = 0;
    private PlayerMovement player; 
    private bool playerHasWon = false;
    private bool playerHasLost = false;

    private Timer gameTimer;

    private void Start()
    {
        victoryScreen.SetActive(false);
        lossScreen.SetActive(false);

        // Find the PlayerMovement script
        player = FindObjectOfType<PlayerMovement>();

        // Find the Timer script
        gameTimer = FindObjectOfType<Timer>();

        // Select the first button
        SelectButton();
    }

    private void Update()
    {
        // Check if the player has won
        if (player.crownCollected && Vector2.Distance(player.transform.position, victoryLocation) < 0.5f)
        {
            playerHasWon = true;
            victoryScreen.SetActive(true);
            PauseController.canPause = false;
        }

        // Check if the player has lost
        if (gameTimer.timeRemaining <= 0 && !playerHasWon)
        {
            playerHasLost = true;
            lossScreen.SetActive(true);
            PauseController.canPause = false;
        }

        if (playerHasWon || playerHasLost)
        {
            int previousSelectedIndex = selectedButtonIndex;

            // Move selection up
            if (Input.GetKeyDown(KeyCode.W))
            {
                if (selectedButtonIndex > 0)
                    selectedButtonIndex--;
                else
                    selectedButtonIndex = menuButtons.Count - 1; // Loop to bottom if at top
            }
            // Move selection down
            else if (Input.GetKeyDown(KeyCode.S))
            {
                if (selectedButtonIndex < menuButtons.Count - 1)
                    selectedButtonIndex++;
                else
                    selectedButtonIndex = 0; // Loop to top if at bottom
            }

            if (previousSelectedIndex != selectedButtonIndex)
                SelectButton();

            // Select option
            if (Input.GetKeyDown(KeyCode.K))
            {
                if (selectedButtonIndex == 0)
                {
                    // Restart game
                    SceneManager.LoadScene("MainGame");
                }
                else if (selectedButtonIndex == menuButtons.Count - 1)
                {
                    // Go to main menu
                    SceneManager.LoadScene("MainMenu");
                }
            }
        }
    }

    private void SelectButton()
    {
        for (int i = 0; i < menuButtons.Count; i++)
        {
            if (i == selectedButtonIndex)
            {
                // Highlight selected button
                Color newColor = new Color32(222, 158, 95, 255);
                menuButtons[i].GetComponentInChildren<TMPro.TextMeshProUGUI>().color = newColor;
            }
            else
            {
                // Unhighlight other buttons
                menuButtons[i].GetComponentInChildren<TMPro.TextMeshProUGUI>().color = Color.gray;
            }
        }
    }
}