using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseFunction : MonoBehaviour
{
    [SerializeField] private GameObject _pausePanel;
    [SerializeField] private PlayerController _playerController;

    public bool IsPaused => _isPaused;
    private bool _isPaused = false;
    private bool _forcePaused = false;

    private void Start()
    {
        Continue();
    }

    public void Pause()
    {
        if (_forcePaused) return;

        _isPaused = !_isPaused;
        if (_isPaused)
        {
            _playerController.StopMovement();
            if (_pausePanel != null) _pausePanel.SetActive(true);
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
        if (_pausePanel != null) _pausePanel.SetActive(false);
        Time.timeScale = 1;
    }

    public void PauseNoMenu()
    {
        _playerController.StopMovement();
        _isPaused = true;
        _forcePaused = true;
        Time.timeScale = 0;
    }
}
