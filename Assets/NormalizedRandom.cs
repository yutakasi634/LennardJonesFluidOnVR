using System.Security.Cryptography;
using UnityEngine;

public class NormalizedRandom
{
    private bool  has_stock;
    private float stock;

    public NormalizedRandom()
    {
        has_stock = false;
    }
    public float Generate(float mean, float standard_deviation)
    {
        if (!has_stock)
        {
            // generate normalized random number by box muller method.
            // Normalized distribution is mean = 0.0. variance = 1.0.
            float x = Random.Range(0.0f, 1.0f);
            float y = Random.Range(0.0f, 1.0f);
            float random = Mathf.Sqrt(-2.0f * Mathf.Log(x)) * Mathf.Cos(2.0f * Mathf.PI * y);
            stock        = Mathf.Sqrt(-2.0f * Mathf.Log(x)) * Mathf.Sin(2.0f * Mathf.PI * y);
            has_stock = true;
            return random * standard_deviation + mean;
        }
        else
        {
            has_stock = false;
            return stock * standard_deviation + mean;
        }
    }
}
