using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class InstantiationExample : MonoBehaviour
{
    // Reference to the Prefab. Drag a Prefab into this field in the Inspector.
    public GameObject myPrefab;
    public Rigidbody projectile;
    public float speed = 4;
    
    // Start is called before the first frame update
    void Start()
    {
        // Instantiate at position (0, 0, 0) and zero rotation.
        // GameObject p = Instantiate(myPrefab, new Vector3(1.0f, 1.0f, 1.0f), Quaternion.identity);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            Rigidbody p = Instantiate(projectile, transform.position, transform.rotation);
            Debug.Log(p.position[0]);
            p.velocity = transform.forward * speed;
        }
    }
}
