using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using Nett;

public class InitialConfGenerator : MonoBehaviour
{
    public float temperature        = 300.0f;
    public float kb                 = 0.8317e-4f; // mass:Da, length:Å, time:0.01ps
    public float box_size           = 5.0f;

    public LennardJonesParticle m_LJParticle;

    private NormalizedRandom m_NormalizedRandom;
    private SystemManager    m_SystemManager;

    // Start is called before the first frame update
    void Start()
    {
        // read input file
        string input_file_path = Application.dataPath + "/../input/system.toml";
        TomlTable root = Toml.ReadFile(input_file_path);
        List<TomlTable> particles = root.Get<List<TomlTable>>("particles");

        // generate initial particle position
        m_SystemManager = GetComponent<SystemManager>();
        m_SystemManager.ljparticles = new List<LennardJonesParticle>();

        foreach (TomlTable particle_info in particles)
        {
            float[] position = particle_info.Get<float[]>("pos");
            LennardJonesParticle new_particle =
                Instantiate(m_LJParticle,
                            new Vector3(position[0], position[1], position[2]),
                            transform.rotation);
            m_SystemManager.ljparticles.Add(new_particle);
        }
        Debug.Log("Initial particle positions were generated.");

        // generate initial particle velocity
        m_NormalizedRandom = new NormalizedRandom();
        foreach (LennardJonesParticle lj_part in m_SystemManager.ljparticles)
        {
            Rigidbody new_rigid = lj_part.GetComponent<Rigidbody>();
            float sigma = Mathf.Sqrt(kb * temperature / new_rigid.mass);
            new_rigid.velocity = new Vector3(m_NormalizedRandom.Generate(0.0f, sigma),
                                             m_NormalizedRandom.Generate(0.0f, sigma),
                                             m_NormalizedRandom.Generate(0.0f, sigma));
        }
        Debug.Log("Initial particle velocities were generated.");

        // Initialize SystemManager
        m_SystemManager.Init(box_size);
        Debug.Log("SystemManager initialized.");
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
            new_rigid.velocity  = new Vector3(m_NormalizedRandom.Generate(0.0f, sigma),
                                              m_NormalizedRandom.Generate(0.0f, sigma),
                                              m_NormalizedRandom.Generate(0.0f, sigma));
            m_SystemManager.ljparticles.Add(new_particle);
        }
    }
}
