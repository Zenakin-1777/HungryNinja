using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonManager : MonoBehaviour
{
    public GameManager gameManager;
    public GameObject settingsMenu;
    public GameObject gameOverMenu;
    public GameObject mainMenu;
    private int previousMenu;

    // Start is called before the first frame update
    private void Start()
    {
        gameManager = GameObject.Find("Game Manager").GetComponent<GameManager>();
    }

    public void LeaveButton()
    {
        Application.Quit();
    }

    public void OpenSettingsButton()
    {
        if (gameOverMenu.activeSelf)
        {
            settingsMenu.SetActive(true);
            gameOverMenu.SetActive(false);
            previousMenu = 1;
        }
        else if (mainMenu.activeSelf)
        {
            settingsMenu.SetActive(true);
            mainMenu.SetActive(false);
            previousMenu = 2;
        }
    }

    public void CloseSettingsButton()
    {
        if (previousMenu == 1)
        {
            settingsMenu.SetActive(false);
            gameOverMenu.SetActive(true);
            previousMenu = 0;
            gameManager.escaped = false;
        }
        else if (previousMenu == 2)
        {
            settingsMenu.SetActive(false);
            mainMenu.SetActive(true);
            previousMenu = 0;
            gameManager.escaped = false;
        }
        else
        {
            settingsMenu.SetActive(false);
            gameManager.escaped = false;
            Time.timeScale = 1;
        }
    }
}
