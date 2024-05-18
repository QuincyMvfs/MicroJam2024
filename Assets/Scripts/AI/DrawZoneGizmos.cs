using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawZoneGizmos : MonoBehaviour
{
    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Collider collider = GetComponent<Collider>();

        if (collider is SphereCollider sphereCollider)
        {
            Gizmos.DrawWireSphere(sphereCollider.bounds.center, sphereCollider.radius);
        }
    }
}
