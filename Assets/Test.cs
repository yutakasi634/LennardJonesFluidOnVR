using System.Security.Cryptography;
using UnityEngine;

public class Test : MonoBehaviour
{
    public string myName;
    private RandomNumberGenerator randGen;
    private Rigidbody rb;
    private float box_size;
    
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        box_size = 3.0f;
        Debug.Log("I'm alive and my name is " + myName);
        Debug.Log("Box size is " + box_size.ToString());
        rb.useGravity = false;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 vel_diff = new Vector3(Random.Range(-1.0f, 1.0f), 
                                       Random.Range(-1.0f, 1.0f), 
                                       Random.Range(-1.0f, 1.0f));
        rb.velocity += vel_diff;
        rb.velocity -= 0.1f * rb.velocity.magnitude * rb.velocity;

        // fix by Periodic Boundary Condition
        Vector3 currentPos = transform.position;
        if (Mathf.Abs(currentPos.x) > box_size)
        {
            currentPos.x -= 2 * Mathf.Sign(currentPos.x) * box_size;
        }
        if(Mathf.Abs(currentPos.y) > box_size)
        {
            currentPos.y -= 2 * Mathf.Sign(currentPos.y) * box_size;
        }
        if (Mathf.Abs(currentPos.z) > box_size)
        {
            currentPos.z -= 2 * Mathf.Sign(currentPos.z) * box_size;
        }
        transform.position = currentPos;
    }
}