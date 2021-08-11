using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class mainMenu : MonoBehaviour
{
    public GameObject mainMenuPanel;
    public GameObject optionPanel;
    public void startGame()
    {
        SceneManager.LoadScene("DemoScene_01");
    }
    public void optionMenu()
    {
        mainMenuPanel.SetActive(false);
        optionPanel.SetActive(true);
    }
    public void backMenu()
    {
        mainMenuPanel.SetActive(true);
        optionPanel.SetActive(false);
    }
    public void quitGame()
    {
        Application.Quit();
    }
}
