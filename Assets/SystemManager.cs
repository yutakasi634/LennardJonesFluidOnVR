using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SystemManager : MonoBehaviour
{
    public Text SystemMonitorPanel;

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

    internal void Init()
    {
        UpdateKineticEnergy();
    }

    internal void UpdateKineticEnergy()
    {
        kinetic_ene = 0.0f;
        foreach (LennardJonesParticle lj_part in ljparticles)
        {
            Rigidbody lj_Rigidbody = lj_part.GetComponent<Rigidbody>();
            kinetic_ene += Mathf.Pow(lj_Rigidbody.velocity.magnitude, 2.0f) * lj_Rigidbody.mass * 0.5f;
        }
    }
}