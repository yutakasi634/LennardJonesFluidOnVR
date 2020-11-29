using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SystemManager : MonoBehaviour
{
    private float m_BoxSize;
    private List<LennardJonesParticle> m_LJParticles;

    private float kinetic_ene;

    internal SystemManager(List<LennardJonesParticle> ljparticles, float box_size)
    {
        m_BoxSize = box_size;
        m_LJParticles = ljparticles;
        UpdateKineticEnergy();
    }

    private void FixedUpdate()
    {
        foreach (LennardJonesParticle lj_part in m_LJParticles)
        {
            // fix by Reflecting Boundary Condition
            Rigidbody lj_rigid = lj_part.GetComponent<Rigidbody>();
            Vector3 currentPos = lj_rigid.position;
            Vector3 currentVel = lj_rigid.velocity;
            if (Mathf.Abs(currentPos.x) > m_BoxSize)
            {
                currentVel.x = -currentVel.x;
            }
            if (Mathf.Abs(currentPos.y) > m_BoxSize)
            {
                currentVel.y = -currentVel.y;
            }
            if (Mathf.Abs(currentPos.z) > m_BoxSize)
            {
                currentVel.z = -currentVel.z;
            }
            lj_rigid.velocity = currentVel;
        }
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