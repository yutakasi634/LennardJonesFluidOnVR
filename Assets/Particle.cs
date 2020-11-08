using System.Security.Cryptography;
using UnityEngine;

public class Particle : MonoBehaviour
{
    public float sphere_radius;
    public float epsilon;
    public float temperature;
    public float mass;

    private float sigma; // sigma for gaussian in Maxwell distribution
    private float kb;
    private Rigidbody      m_Rigidbody;
    private SphereCollider m_SphereCollider;
    private float box_size;
    private float force_range;
    private float dump_coef;
    
    // Start is called before the first frame update
    void Start()
    {
        m_Rigidbody       = GetComponent<Rigidbody>();
        m_SphereCollider  = GetComponent<SphereCollider>();
        sphere_radius              = 0.5f;
        m_SphereCollider.radius    = 2.5f * sphere_radius;
        m_SphereCollider.isTrigger = true;
        box_size    = 5.0f;
        //dump_coef   = 1.0f;
        epsilon     = 0.05f;
        m_Rigidbody.useGravity = false;
        mass                   = 1.0f;
        m_Rigidbody.mass       = mass;
    }

    // Update is called once per frame
    void Update()
    {
    }

    void FixedUpdate()
    {
        //Vector3 force_vec -= dump_coef * m_Rigidbody.velocity.magnitude * m_Rigidbody.velocity;
        //m_Rigidbody.AddForce(force_vec); 

        // fix by Periodic Boundary Condition
        Vector3 currentPos = transform.position;
        Vector3 currentVel = m_Rigidbody.velocity;
        if (Mathf.Abs(currentPos.x) > box_size)
        {
            currentVel.x = -currentVel.x;
        }
        if(Mathf.Abs(currentPos.y) > box_size)
        {
            currentVel.y = -currentVel.y;
        }
        if (Mathf.Abs(currentPos.z) > box_size)
        {
            currentVel.z = -currentVel.z;
        }
        m_Rigidbody.velocity = currentVel;
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
}
