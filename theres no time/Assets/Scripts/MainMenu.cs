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

        // move selection up
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            if (selectedButtonIndex > 0)
                selectedButtonIndex--;
            else
                selectedButtonIndex = menuButtons.Count - 1; // loop to bottom if at top
        }
        // move selection down
        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            if (selectedButtonIndex < menuButtons.Count - 1)
                selectedButtonIndex++;
            else
                selectedButtonIndex = 0; // loop to top if at bottom
        }

        if (previousSelectedIndex != selectedButtonIndex)
            SelectButton();

        // select option
        if (Input.GetKeyDown(KeyCode.Keypad4))
        {
            if (selectedButtonIndex == 0)
            {
                SceneManager.LoadScene("IntroSlides");
            }
            else if (selectedButtonIndex == menuButtons.Count - 1)
            {
                Application.Quit();
            }
            {
                // eughnjkfdg. controls/settings go here :(
            }
        }
    }

    private void SelectButton()
    {
        for (int i = 0; i < menuButtons.Count; i++)
        {
            if (i == selectedButtonIndex)
            {
                // highlight current button
                Color newColor = new Color32(222, 158, 95, 255);
                menuButtons[i].GetComponentInChildren<TMPro.TextMeshProUGUI>().color = newColor;
            }
            else
            {
                // return other buttons to grey
                menuButtons[i].GetComponentInChildren<TMPro.TextMeshProUGUI>().color = Color.gray;
            }
        }
    }
}