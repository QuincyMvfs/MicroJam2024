using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZoneIndicator : MonoBehaviour
{
    [SerializeField] private GameObject _zoneIndicator;
    [SerializeField] private Material _whiteMaterial;
    [SerializeField] private Material _redMaterial;

    private Renderer _objectRenderer;

    private void Start()
    {
        _objectRenderer = _zoneIndicator.GetComponent<Renderer>();
    }

    public void SetIndicatorActive(bool isActive)
    {
        _objectRenderer.material = isActive ? _redMaterial : _whiteMaterial;
    }
}
