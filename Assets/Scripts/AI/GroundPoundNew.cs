using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundPoundNew : MonoBehaviour
{
    [SerializeField] private GameObject[] _damageZones;
    [SerializeField] private GameObject[] _damageZonesVFX;
    [SerializeField] private float _damageAmount = 10.0f;
    [SerializeField] private float _warningWaitTime = 2f;

    [SerializeField] private AudioSource _explosionChargeUpSFX;
    [SerializeField] private AudioSource _explosionSFX;

    public void PerformGroundPound()
    {
        StartCoroutine(PerformGroundSequence());
    }

    IEnumerator PerformGroundSequence()
    {
        for (int i = 0; i < _damageZones.Length; i++)
        {
            // Check if the selected zone is valid
            if (_damageZones[i] == null)
            {
                yield break;
            }

            if (_explosionChargeUpSFX != null)
            {
                AudioSource SpawnedAudio = Instantiate(_explosionChargeUpSFX, transform.position, transform.rotation);
                Destroy(SpawnedAudio, _warningWaitTime);
            }
            // Show the indicator for the selected zone
            ZoneIndicator zone = _damageZones[i].GetComponent<ZoneIndicator>();
            zone.SetIndicatorActive(true);
            
            yield return new WaitForSeconds(_warningWaitTime);

            if (_explosionSFX != null)
            {
                AudioSource SpawnedAudio = Instantiate(_explosionSFX, transform.position, transform.rotation);
                Destroy(SpawnedAudio, 2f);
            }

            ApplyZoneExplosionDamage(i);
            zone.SetIndicatorActive(false);
        }
    }

    void ApplyZoneExplosionDamage(int zoneIndex)
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
        if (_damageZonesVFX[zoneIndex] != null)
        {
            _damageZonesVFX[zoneIndex].SetActive(true);
            StartCoroutine(DeactivateVFXAfterDelay(_damageZonesVFX[zoneIndex], 0.5f));
        }
    }

    IEnumerator DeactivateVFXAfterDelay(GameObject vfx, float delay)
    {
        yield return new WaitForSeconds(delay);
        vfx.SetActive(false);
    }
}
