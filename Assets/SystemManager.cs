using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SystemManager : MonoBehaviour
{
    public Text SystemMonitorPanel;

    private float m_BoxSize;
    internal List<LennardJonesParticle> ljparticles;

    private float kinetic_ene;

    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating("UpdateKineticEnergy", 0.1f, 0.1f);
    }

    // Update is called once per frame
    void Update()
    {
        SystemMonitorPanel.text =
            "System status\n" +
            $"particle num : {ljparticles.Count}\n" +
            $"kinetic energy : {kinetic_ene.ToString("F02")}";
    }

    private void FixedUpdate()
    {
        foreach (LennardJonesParticle lj_part in ljparticles)
        {
            // fix by Periodic Boundary Condition
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

    internal void Init(float box_size)
    {
        m_BoxSize = box_size;
        UpdateKineticEnergy();
    }

    internal void UpdateKineticEnergy()
    {
        kinetic_ene = 0.0f;
        foreach (LennardJonesParticle lj_part in ljparticles)
        {
            Rigidbody lj_Rigidbody = lj_part.GetComponent<Rigidbody>();
            kinetic_ene +=
                Mathf.Pow(lj_Rigidbody.velocity.magnitude, 2.0f) * lj_Rigidbody.mass * 0.5f;
        }
    }
}