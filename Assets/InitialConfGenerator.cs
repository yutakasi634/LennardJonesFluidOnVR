using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class InitialConfGenerator : MonoBehaviour
{
    public float temperature        = 300.0f;
    public float kb                 = 0.8317e-4f; // mass:Da, length:Å, time:0.01ps
    public float box_size           = 5.0f;
    public uint  total_particle_num = 500;

    public LennardJonesParticle m_LJParticle;

    private SystemManager m_SystemManager;
    private uint max_trial_num      = 500;

    // Start is called before the first frame update
    void Start()
    {
        m_SystemManager = GetComponent<SystemManager>();
        m_SystemManager.ljparticles = new List<LennardJonesParticle>();
        List<LennardJonesParticle> lj_part_list = m_SystemManager.ljparticles;
        // generate initial particle positions
        uint trial_num               = 0;
        while (lj_part_list.Count < total_particle_num && trial_num < max_trial_num)
        {
            trial_num += 1;
            Vector3 new_coord = new Vector3(Random.Range(-box_size, box_size),
                                            Random.Range(-box_size, box_size),
                                            Random.Range(-box_size, box_size));

            // check collision with existing particles
            bool acceptable = true;
            if (lj_part_list.Count == 0)
            {
                LennardJonesParticle new_particle =
                    Instantiate(m_LJParticle, new_coord, transform.rotation);
                lj_part_list.Add(new_particle);
                continue;
            }

            float check_range = lj_part_list[0].sphere_radius * 2;
            foreach (LennardJonesParticle fixed_part in lj_part_list)
            {
                float distance = (fixed_part.transform.position - new_coord).magnitude;
                if (distance < check_range)
                { 
                    acceptable = false;
                    break;
                }
            }
            if (acceptable)
            {
                LennardJonesParticle new_particle =
                    Instantiate(m_LJParticle, new_coord, transform.rotation);
                lj_part_list.Add(new_particle);
                trial_num = 0;
            }
        }

        // generate initial particle velocity
        foreach (LennardJonesParticle lj_part in lj_part_list)
        {
            Rigidbody new_rigid = lj_part.GetComponent<Rigidbody>();
            float sigma = Mathf.Sqrt(kb * temperature / new_rigid.mass);
            new_rigid.velocity = new Vector3(NormalizedRandom.Generate(0.0f, sigma),
                                             NormalizedRandom.Generate(0.0f, sigma),
                                             NormalizedRandom.Generate(0.0f, sigma));

        }

        // Initialize SystemManager
        m_SystemManager.Init(box_size);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            LennardJonesParticle new_particle =
                Instantiate(m_LJParticle, transform.position, transform.rotation);
            Rigidbody new_rigid = new_particle.GetComponent<Rigidbody>();
            float sigma = Mathf.Sqrt(kb * temperature / new_rigid.mass);
            new_rigid.velocity  = new Vector3(NormalizedRandom.Generate(0.0f, sigma),
                                              NormalizedRandom.Generate(0.0f, sigma),
                                              NormalizedRandom.Generate(0.0f, sigma));
            m_SystemManager.ljparticles.Add(new_particle);
        }
    }
}
