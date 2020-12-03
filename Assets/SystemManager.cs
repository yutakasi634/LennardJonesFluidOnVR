using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SystemManager : MonoBehaviour
{
    private Vector3 m_UpperBoundary;
    private Vector3 m_LowerBoundary;
    private List<LennardJonesParticle> m_LJParticles;

    private float kinetic_ene;

    private void FixedUpdate()
    {
        foreach (LennardJonesParticle lj_part in m_LJParticles)
        {
            // fix by Reflecting Boundary Condition
            Rigidbody lj_rigid = lj_part.GetComponent<Rigidbody>();
            Vector3 currentPos = lj_rigid.position;
            Vector3 currentVel = lj_rigid.velocity;
            if (currentPos.x < m_LowerBoundary.x || m_UpperBoundary.x < currentPos.x)
            {
                currentVel.x = -currentVel.x;
            }
            if (currentPos.y < m_LowerBoundary.y || m_UpperBoundary.y < currentPos.y)
            {
                currentVel.y = -currentVel.y;
            }
            if (currentPos.z < m_LowerBoundary.z || m_UpperBoundary.z < currentPos.z)
            {
                currentVel.z = -currentVel.z;
            }
            lj_rigid.velocity = currentVel;
        }
    }

    internal void Init (List<LennardJonesParticle> ljparticles, Vector3 upper_boundary, Vector3 lower_boundary)
    {
        m_UpperBoundary = upper_boundary;
        m_LowerBoundary = lower_boundary;
        m_LJParticles = ljparticles;
        UpdateKineticEnergy();
    }

    internal void UpdateKineticEnergy()
    {
        kinetic_ene = 0.0f;
        foreach (LennardJonesParticle lj_part in m_LJParticles)
        {
            Rigidbody lj_Rigidbody = lj_part.GetComponent<Rigidbody>();
            kinetic_ene +=
                Mathf.Pow(lj_Rigidbody.velocity.magnitude, 2.0f) * lj_Rigidbody.mass * 0.5f;
        }
    }
}