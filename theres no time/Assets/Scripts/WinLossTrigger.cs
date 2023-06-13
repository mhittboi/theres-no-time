using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using TMPro;

public class WinLossTrigger : MonoBehaviour
{
    public GameObject victoryScreen;
    public List<Button> victoryMenuButtons;

    public GameObject lossScreen;
    public List<Button> lossMenuButtons;

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

        // source variables for determining win/loss from other scripts
        player = FindObjectOfType<PlayerMovement>();
        gameTimer = FindObjectOfType<Timer>();

        // start with the first button highlighted
        SelectButton();
    }

    private void Update()
    {
        // check if the player has won
        if (player.crownCollected && Vector2.Distance(player.transform.position, victoryLocation) < 0.5f && !playerHasLost)
        {
            playerHasWon = true;
            victoryScreen.SetActive(true);
            PauseController.canPause = false;
            SelectButton();
        }

        // check if the player has lost
        if (gameTimer.timeRemaining <= 0 && !playerHasWon)
        {
            playerHasLost = true;
            lossScreen.SetActive(true);
            PauseController.canPause = false;
            SelectButton();
        }

        if (playerHasWon || playerHasLost)
        {
            int previousSelectedIndex = selectedButtonIndex;

            if (Input.GetKeyDown(KeyCode.UpArrow)) // move selection up
            {
                selectedButtonIndex--;
            }
            else if (Input.GetKeyDown(KeyCode.DownArrow)) // move selection down
            {
                selectedButtonIndex++;
            }

            if (previousSelectedIndex != selectedButtonIndex)
                SelectButton();

            // Select option
            if (Input.GetKeyDown(KeyCode.Keypad4))
            {
                if (selectedButtonIndex == 0)
                {
                    SceneManager.LoadScene("MainGame");
                }
                else if (selectedButtonIndex == (playerHasWon ? victoryMenuButtons.Count - 1 : lossMenuButtons.Count - 1))
                {
                    SceneManager.LoadScene("MainMenu");
                }
            }
        }
    }

    private void SelectButton()
    {
        List<Button> activeMenuButtons = playerHasWon ? victoryMenuButtons : lossMenuButtons;

        for (int i = 0; i < activeMenuButtons.Count; i++)
        {
            if (i == selectedButtonIndex)
            {
                // Highlight selected button
                Color newColor = new Color32(222, 158, 95, 255);
                activeMenuButtons[i].GetComponentInChildren<TMPro.TextMeshProUGUI>().color = newColor;
            }
            else
            {
                // Unhighlight other buttons
                activeMenuButtons[i].GetComponentInChildren<TMPro.TextMeshProUGUI>().color = Color.gray;
            }
        }
    }
}