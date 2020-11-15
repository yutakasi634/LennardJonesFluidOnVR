using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SystemManager : MonoBehaviour
{
    public Text SystemMonitorPanel;

    internal List<LennardJonesParticle> ljparticles;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        SystemMonitorPanel.text =
            $"System status\nparticle num : {ljparticles.Count}";
    }
}