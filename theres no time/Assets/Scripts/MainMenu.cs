using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using TMPro;

public class MainMenu : MonoBehaviour
{
    public List<Button> menuButtons;
    private int selectedButtonIndex = 0;

    private void Start()
    {
        SelectButton();
    }

    private void Update()
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
                SceneManager.LoadScene("IntroSlides");
            }
            else if (selectedButtonIndex == menuButtons.Count - 1)
            {
                // Application.Quit;
            }
            {
                // Start game or do something else depending on selectedButtonIndex
                // SceneManager.LoadScene("YourSceneName");
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