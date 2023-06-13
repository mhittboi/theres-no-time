using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using TMPro;
using UnityEngine.SceneManagement;

public class PauseController : MonoBehaviour
{
    public List<Button> menuButtons;
    public List<TextMeshProUGUI> menuButtonLabels;
    public GameObject pauseGrid;
    public GameObject settingsGrid;
    public GameObject controlsGrid;

    private int selectedButtonIndex = 0;
    private bool isPaused = false;
    public static bool canPause = true;

    private enum MenuState { PAUSED, SETTINGS, CONTROLS }
    private MenuState currentMenuState = MenuState.PAUSED;

    private void Start()
    {
        InitializeMenu();
    }

    private void Update()
    {
        HandleMenuInput();
    }

    private void InitializeMenu()
    {
        CloseAllMenus();

        foreach (var label in menuButtonLabels)
        {
            label.enabled = false;
        }
    }

    private void HandleMenuInput()
    {
        if (canPause && Input.GetKeyDown(KeyCode.Keypad7))
        {
            TogglePause();
        }

        if (isPaused)
        {
            HandlePauseMenuNavigation();
            if (Input.GetKeyDown(KeyCode.Keypad4))
            {
                SelectMenuOption();
            }
        }
    }

    private void TogglePause()
    {
        if (canPause && isPaused)
        {
            switch (currentMenuState)
            {
                case MenuState.PAUSED:
                    ResumeGame();
                    break;
                case MenuState.SETTINGS:
                case MenuState.CONTROLS:
                    OpenPauseMenu();
                    break;
            }
        }
        else
        {
            PauseGame();
        }
    }

    private void HandlePauseMenuNavigation()
    {
        if (currentMenuState == MenuState.PAUSED)
        {
            int previousSelectedIndex = selectedButtonIndex;

            if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                selectedButtonIndex = (selectedButtonIndex > 0) ? selectedButtonIndex - 1 : menuButtons.Count - 1;
            }
            else if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                selectedButtonIndex = (selectedButtonIndex < menuButtons.Count - 1) ? selectedButtonIndex + 1 : 0;
            }

            if (previousSelectedIndex != selectedButtonIndex)
            {
                SelectButton();
            }
        }
    }

    private void SelectMenuOption()
    {
        switch (selectedButtonIndex)
        {
            case 0:
                NavigateSettings();
                break;
            case 1:
                NavigateControls();
                break;
            case 2:
                NavigateMainMenu();
                break;
        }
    }

    private void PauseGame()
    {
        Time.timeScale = 0f;
        isPaused = true;
        OpenPauseMenu();
    }

    public void ResumeGame()
    {
        Time.timeScale = 1f;
        isPaused = false;
        CloseAllMenus();
    }

    private void OpenPauseMenu()
    {
        CloseAllMenus();
        pauseGrid.SetActive(true);
        SelectButton();
        currentMenuState = MenuState.PAUSED;
    }

    private void CloseAllMenus()
    {
        pauseGrid.SetActive(false);
        settingsGrid.SetActive(false);
        controlsGrid.SetActive(false);
        DisablePauseMenuLabels();
    }

    private void SelectButton()
    {
        Color selectedColor = new Color32(222, 158, 95, 255);

        for (int i = 0; i < menuButtons.Count; i++)
        {
            menuButtonLabels[i].enabled = true;
            menuButtonLabels[i].color = (i == selectedButtonIndex) ? selectedColor : Color.gray;
        }
    }

    public void NavigateSettings()
    {
        CloseAllMenus();
        settingsGrid.SetActive(true);
        currentMenuState = MenuState.SETTINGS;
    }

    public void NavigateControls()
    {
        CloseAllMenus();
        controlsGrid.SetActive(true);
        currentMenuState = MenuState.CONTROLS;
    }

    private void NavigateMainMenu()
    {
        ResumeGame();
        SceneManager.LoadScene("MainMenu");
    }

    private void DisablePauseMenuLabels()
    {
        foreach (var label in menuButtonLabels)
        {
            label.enabled = false;
        }
    }
}