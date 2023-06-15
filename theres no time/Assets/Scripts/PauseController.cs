using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using TMPro;
using UnityEngine.SceneManagement;

public class PauseController : MonoBehaviour
{
    // menus for ui buttons :| (evil)
    public List<Button> menuButtons;
    public List<TextMeshProUGUI> menuButtonLabels;

    // the different ui parts
    public GameObject pauseGrid;
    public GameObject settingsGrid;
    public GameObject controlsGrid;

    // pause settings + selected button
    private int selectedButtonIndex = 0;
    public static bool isPaused = false;
    public static bool canPause = true;

    // different ui possible states
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
        // make sure game starts with all menus & labels OFF!!!!!!!!!!!!!!!
        CloseAllMenus();
        foreach (var label in menuButtonLabels)
        {
            label.enabled = false;
        }
    }

    private void HandleMenuInput()
    {
        // first check if player is pausing
        if (canPause && Input.GetKeyDown(KeyCode.Keypad7))
        {
            TogglePause();
        }

        // then check if it is paused and send to menu function if it is :3
        if (isPaused)
        {
            HandlePauseMenuNavigation();
            // select users current option
            if (Input.GetKeyDown(KeyCode.Keypad4))
            {
                SelectMenuOption();
            }
        }
    }

    // pause controller
    private void TogglePause()
    {
        if (canPause && isPaused)
        {
            switch (currentMenuState)
            {
                case MenuState.PAUSED: // if game is currently paused, toggle to resume
                    ResumeGame();
                    break;
                case MenuState.SETTINGS: // if settings or menu is opened, return to main pause screen
                case MenuState.CONTROLS:
                    OpenPauseMenu();
                    break;
            }
        }
        else
        {
            PauseGame(); // otherwise pause the game
        }
    }

    // menu navigator
    private void HandlePauseMenuNavigation()
    {
        if (currentMenuState == MenuState.PAUSED) // only functions while the player is on the pause screen
        {
            int previousSelectedIndex = selectedButtonIndex;

            if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                selectedButtonIndex = (selectedButtonIndex > 0) ? selectedButtonIndex - 1 : menuButtons.Count - 1; // selection -1
            }
            else if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                selectedButtonIndex = (selectedButtonIndex < menuButtons.Count - 1) ? selectedButtonIndex + 1 : 0; // selection +1
            }

            if (previousSelectedIndex != selectedButtonIndex)
            {
                SelectButton();
            }
        }
    }

    // different menu options for the button index
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

    // setting menu base
    public void NavigateSettings()
    {
        CloseAllMenus();
        settingsGrid.SetActive(true);
        currentMenuState = MenuState.SETTINGS;
    }

    // controls menu base
    public void NavigateControls()
    {
        CloseAllMenus();
        controlsGrid.SetActive(true);
        currentMenuState = MenuState.CONTROLS;
    }

    // return to main menu & resume game so next one starts appropriately
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