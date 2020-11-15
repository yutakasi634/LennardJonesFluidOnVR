using System;
using UnityEngine;

public class LennardJonesParticle : MonoBehaviour
{
    public float sphere_radius = 0.5f;
    public float epsilon       = 0.05f;

    private Rigidbody      m_Rigidbody;
    private SphereCollider m_SphereCollider;

    // Start is called before the first frame update
    void Start()
    {
        m_Rigidbody       = GetComponent<Rigidbody>();
        m_SphereCollider  = GetComponent<SphereCollider>();
        // This radius mean cutoff radius
        m_SphereCollider.radius    = 2.5f * sphere_radius;
        m_SphereCollider.isTrigger = true;
        m_Rigidbody.useGravity     = false;
    }

    void OnTriggerStay(Collider other)
    {
        Vector3 dist_vec = other.attachedRigidbody.position - transform.position;
        float rinv       = 1.0f / dist_vec.magnitude;
        float r1s1       = sphere_radius * rinv;
        float r3s3       = r1s1 * r1s1* r1s1;
        float r6s6       = r3s3 * r3s3;
        float r12s12     = r6s6 * r6s6;
        float derivative = 24.0f * epsilon * (r6s6 - 2.0f * r12s12) * rinv;
        m_Rigidbody.AddForce(derivative * rinv * dist_vec);
    }
};
