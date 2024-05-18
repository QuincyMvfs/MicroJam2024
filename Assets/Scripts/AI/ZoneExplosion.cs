using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ZoneExplosion : MonoBehaviour
{
    [SerializeField] private GameObject[] _damageZones;
    [SerializeField] private float _damageAmount = 10.0f;
    [SerializeField] private float _warningWaitTime = 2f;

    [Tooltip("Make 2 zones to apply damage if health below this threshold")]
    [SerializeField] private float _healthThreshold = 50.0f;

    private AIController _aiController;
    private Transform _playerTransform;

    private HealthComponent _healthComponent;

    private void Start()
    {
        _aiController = GetComponent<AIController>();
        _healthComponent = GetComponent<HealthComponent>();
        PlayerController Player = FindObjectOfType<PlayerController>();
        if (Player != null)
        {
           _playerTransform = Player.gameObject.transform;
        }
    }

    public void PerformGroundPound()
    {
        ActivateZones(true);
        StartCoroutine(GroundPoundSequence());
    }

    void ActivateZones(bool activate)
    {
        // Activate all children of each damage zone
        foreach (GameObject zone in _damageZones)
        {
            for (int i = 0; i < zone.transform.childCount; i++)
            {
                Transform child = zone.transform.GetChild(i);
                child.gameObject.SetActive(activate);
            }
        }
    }

    IEnumerator GroundPoundSequence()
    {
        int zoneIndex = DeterminePlayerZone();

        // Check if the selected zone is valid
        if (_damageZones[zoneIndex] == null)
        {
            yield break;
        }

        // Show the indicator for the selected zone
        ZoneIndicator zone = _damageZones[zoneIndex].GetComponent<ZoneIndicator>();
        zone.SetIndicatorActive(true);

        int secondZoneIndex = -1;
        if (_healthComponent != null && _healthComponent.CurrentHealth < _healthThreshold)
        {
            // Select a second random zone that is different from the first one
            secondZoneIndex = zoneIndex;
            while (secondZoneIndex == zoneIndex || _damageZones[secondZoneIndex] == null)
            {
                secondZoneIndex = Random.Range(0, _damageZones.Length);
            }

            // Show the indicator for the second zone
            ZoneIndicator secondZone = _damageZones[secondZoneIndex].GetComponent<ZoneIndicator>();
            secondZone.SetIndicatorActive(true);
        }

        // Wait for a short duration to give the player a warning
        yield return new WaitForSeconds(_warningWaitTime);

        // Deactivate indicators
        zone.SetIndicatorActive(false);
        if (secondZoneIndex != -1)
        {
            ZoneIndicator secondZone = _damageZones[secondZoneIndex].GetComponent<ZoneIndicator>();
            secondZone.SetIndicatorActive(false);
        }

        // Activate zones and apply damage
        ActivateZones(false);
        ApplyGroundPoundDamage(zoneIndex);
        if (secondZoneIndex != -1)
        {
            ApplyGroundPoundDamage(secondZoneIndex);
        }

        _aiController.ResetToIdle();
    }

    int DeterminePlayerZone()
    {
        float distanceToCenter = Vector3.Distance(_playerTransform.position, this.transform.position);

        for (int i = 0; i < _damageZones.Length; i++)
        {
            SphereCollider zoneCollider = _damageZones[i].GetComponent<SphereCollider>();
            if (distanceToCenter <= zoneCollider.radius)
            {
                return i;
            }
        }

        return 1; 
    }

    // TODO:: Maybe should trigger from anim notify????????????????
    void ApplyGroundPoundDamage(int zoneIndex)
    {
        SphereCollider selectedZoneCollider = _damageZones[zoneIndex].GetComponent<SphereCollider>();
        if (selectedZoneCollider == null) return;

        float selectedZoneRadius = selectedZoneCollider.radius;
        float innerZoneRadius = 0;
        if (zoneIndex > 0)
        {
            innerZoneRadius = _damageZones[zoneIndex - 1].GetComponent<SphereCollider>().radius;
        }

        Collider[] hitColliders = Physics.OverlapSphere(selectedZoneCollider.bounds.center, selectedZoneRadius);

        foreach (Collider hitCollider in hitColliders)
        {
            if (hitCollider.gameObject == this.gameObject) continue;

            Vector3 hitPosition = hitCollider.transform.position;
            hitPosition.y = 0;
            Vector3 startPosition = this.transform.position;
            startPosition.y = 0;

            float distanceToCenter = Vector3.Distance(hitPosition, startPosition);
            distanceToCenter = Mathf.RoundToInt(distanceToCenter);

            // Apply damage only if the target is within the selected zone's radius and outside the inner zone
            if (distanceToCenter <= selectedZoneRadius && distanceToCenter > innerZoneRadius)
            {
                HealthComponent targetHealth = hitCollider.GetComponent<HealthComponent>();
                if (targetHealth != null)
                {
                    targetHealth.Damage(_damageAmount, this.gameObject);
                }
            }
        }
    }
}
