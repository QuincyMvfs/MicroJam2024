using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZoneIndicator : MonoBehaviour
{
    [SerializeField] private GameObject _zoneIndicator;
    public void SetIndicatorActive(bool isActive)
    {
        _zoneIndicator.SetActive(isActive);
    }
}
