using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseFunction : MonoBehaviour
{
    [SerializeField] private GameObject _pausePanel;
    [SerializeField] private PlayerController _playerController;

    public bool IsPaused => _isPaused;
    private bool _isPaused = false;

    public void Pause()
    {
        _isPaused = !_isPaused;
        if (_isPaused)
        {
            _playerController.StopMovement();
            Debug.Log("paused");
            _pausePanel.SetActive(true);
            Time.timeScale = 0;
        }
        else
        {
            Continue();
        }
    }

    public void Continue()
    {
        _isPaused = false;
        Debug.Log("unpaused");
        _pausePanel.SetActive(false);
        Time.timeScale = 1;
    }
}
