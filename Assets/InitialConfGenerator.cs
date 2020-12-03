using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Assertions;
using Nett;

public class InitialConfGenerator : MonoBehaviour
{
    public LennardJonesParticle m_LJParticle;

    private float temperature = 300.0f;
    private float kb = 0.8317e-4f; // mass:Da, length:Å, time:0.01ps
    private NormalizedRandom m_NormalizedRandom;
    private SystemManager    m_SystemManager;

    // Start is called before the first frame update
    void Start()
    {
        // read input file
        string input_file_path = Application.dataPath + "/../input/lennard-jones.toml";
        TomlTable root = Toml.ReadFile(input_file_path);

        // generate initial particle position, velocity and system temperature
        List<TomlTable> systems                = root.Get<List<TomlTable>>("systems");
        if (2 <= systems.Count)
        {
            throw new System.Exception($"There are {systems.Count} systems. the multiple systems case is not supported.");
        }
        List<LennardJonesParticle> ljparticles = new List<LennardJonesParticle>();
        float[] upper_boundary = new float[3];
        float[] lower_boundary = new float[3];
        m_NormalizedRandom                     = new NormalizedRandom();
        foreach (TomlTable system in systems)
        {
            temperature              = system.Get<TomlTable>("attributes").Get<float>("temperature");
            TomlTable boundary_shape = system.Get<TomlTable>("boundary_shape");
            upper_boundary = boundary_shape.Get<float[]>("upper");
            lower_boundary = boundary_shape.Get<float[]>("lower");
            List<TomlTable> particles = system.Get<List<TomlTable>>("particles");
            foreach (TomlTable particle_info in particles)
            {
                // initialize particle position
                float[] position = particle_info.Get<float[]>("pos");
                LennardJonesParticle new_particle =
                    Instantiate(m_LJParticle,
                                new Vector3(position[0], position[1], position[2]),
                                transform.rotation);

                // initialize particle velocity
                Rigidbody new_rigid = new_particle.GetComponent<Rigidbody>();
                new_rigid.mass = particle_info.Get<float>("m");
                if (particle_info.ContainsKey("vel"))
                {
                    float[] velocity = particle_info.Get<float[]>("vel");
                    new_rigid.velocity = new Vector3(velocity[0], velocity[1], velocity[2]);
                }
                else
                {
                    float sigma = Mathf.Sqrt(kb * temperature / new_rigid.mass);
                    new_rigid.velocity = new Vector3(m_NormalizedRandom.Generate(0.0f, sigma),
                                                     m_NormalizedRandom.Generate(0.0f, sigma),
                                                     m_NormalizedRandom.Generate(0.0f, sigma));
                }
                ljparticles.Add(new_particle);
            }
        }
        Debug.Log("System initialization finished.");

        List<TomlTable> ffs        = root.Get<List<TomlTable>>("forcefields");
        foreach (TomlTable ff in ffs)
        {
            List<TomlTable> global_ffs = ff.Get<List<TomlTable>>("global");
            foreach (TomlTable global_ff in global_ffs)
            {
                Assert.AreEqual("LennardJones", global_ff.Get<string>("potential"),
                    "The potential field is only allowed \"LennardJones\". Other potential or null is here.");
                List<TomlTable> parameters = global_ff.Get<List<TomlTable>>("parameters");
                foreach (TomlTable parameter in parameters)
                {
                    int index = parameter.Get<int>("index");
                    float sigma = parameter.Get<float>("sigma");
                    float radius = sigma / 2;
                    ljparticles[index].sphere_radius        = radius;
                    ljparticles[index].epsilon              = parameter.Get<float>("epsilon");
                    ljparticles[index].transform.localScale = new Vector3(sigma, sigma, sigma);
                }
            }
        }

        // Initialize SystemManager
        m_SystemManager = GetComponent<SystemManager>();
        m_SystemManager.Init(ljparticles,
            new Vector3(upper_boundary[0], upper_boundary[1], upper_boundary[2]),
            new Vector3(lower_boundary[0], lower_boundary[1], lower_boundary[2]));
        Debug.Log("SystemManager initialization finished.");
    }
}
