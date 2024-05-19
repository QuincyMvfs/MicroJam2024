using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseFunction : MonoBehaviour
{
    public GameObject PausePanel;

    public void Pause()
    {
        Debug.Log("paused");
        PausePanel.SetActive(true);
        Time.timeScale = 0;
    }

    public void Continue()
    {
        Debug.Log("unpaused");
        PausePanel.SetActive(false);
        Time.timeScale = 1;
    }
}
