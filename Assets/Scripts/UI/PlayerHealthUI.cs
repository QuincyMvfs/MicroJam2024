using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealthUI : MonoBehaviour
{
    [SerializeField] private float _lerpSpeed = 3.0f;

    private Image _playerHealthImage;
    private PlayerController _playerController;
    private HealthComponent _playerHealthComponent;
    private float _currentFillAmount;

    void Start()
    {
        _playerController = FindObjectOfType<PlayerController>();
        _playerHealthImage = GetComponent<Image>();
        if (_playerController != null)
        {
            _playerHealthComponent = _playerController.gameObject.GetComponent<HealthComponent>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(_playerHealthComponent != null )
        {
            float targetFillAmount = _playerHealthComponent.CurrentHealth / _playerHealthComponent.MaxHealth;
            _currentFillAmount = Mathf.Lerp(_currentFillAmount, targetFillAmount, Time.deltaTime * _lerpSpeed);
            _playerHealthImage.fillAmount = _currentFillAmount;
        }
    }

    public void SetFillAmount(float fillamount)
    {
        _playerHealthImage.fillAmount = fillamount;
    }
}
