using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void Playgame()
    {
        SceneManager.LoadScene(1);
    }

    public void LoadMainMenu()
    {
        SceneManager.LoadScene(0);
    }

    public void Quitgame()
    {
        Application.Quit();
    }

    public void RetryLevel()
    {
        SceneManager.LoadScene(1);
    }

}
