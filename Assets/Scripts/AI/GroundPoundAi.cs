using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GroundPoundAi : MonoBehaviour
{
    [SerializeField] private GameObject[] _damageZones;
    [SerializeField] private float _damageAmount = 10.0f;
    [SerializeField] private Transform _playerTransform;

    public void PerformGroundPound()
    {
        StartCoroutine(GroundPoundSequence());
    }

    IEnumerator GroundPoundSequence()
    {
        // Randomly select a damage zone
        int zoneIndex = DeterminePlayerZone();

        // Check if the selected zone is valid
        if (_damageZones[zoneIndex] == null)
        {
            Debug.LogError($"Damage zone at index {zoneIndex} is not assigned in GroundPoundAI");
            yield break;
        }

        // Show the indicator for the selected zone
        ZoneIndicator zone = _damageZones[zoneIndex].GetComponent<ZoneIndicator>();
        zone.SetIndicatorActive(true);

        // Wait for a short duration to give the player a warning
        yield return new WaitForSeconds(1.0f);

        // Delay the damage application to sync with the animation
        yield return new WaitForSeconds(0.5f);

        zone.SetIndicatorActive(false);

        // Apply damage
        ApplyGroundPoundDamage(zoneIndex);
    }

    int DeterminePlayerZone()
    {
        float distanceToCenter = Vector3.Distance(_playerTransform.position, this.transform.position);

        for (int i = 0; i < _damageZones.Length; i++)
        {
            SphereCollider zoneCollider = _damageZones[i].GetComponent<SphereCollider>();
            if (distanceToCenter <= zoneCollider.radius)
            {
                Debug.Log(_damageZones[i]);
                return i;
            }
        }

        return -1; 
    }

    // TODO:: Maybe should trigger from anim notify????????????????
    void ApplyGroundPoundDamage(int zoneIndex)
    {
        SphereCollider selectedZoneCollider = _damageZones[zoneIndex].GetComponent<SphereCollider>();
        if (selectedZoneCollider == null)
        {
            Debug.LogWarning("Selected zone does not have a SphereCollider");
            return;
        }

        float selectedZoneRadius = selectedZoneCollider.radius;
        float innerZoneRadius = zoneIndex > 0 ? _damageZones[zoneIndex - 1].GetComponent<SphereCollider>().radius : 0;

        Collider[] hitColliders = Physics.OverlapSphere(selectedZoneCollider.bounds.center, selectedZoneRadius);

        foreach (Collider hitCollider in hitColliders)
        {
            if (hitCollider.gameObject == this.gameObject) continue;

            float distanceToCenter = Vector3.Distance(hitCollider.transform.position, this.transform.position);

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
