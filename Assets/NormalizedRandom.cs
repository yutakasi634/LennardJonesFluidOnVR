using System.Security.Cryptography;
using UnityEngine;

public class NormalizedRandom
{
    public static float Generate(float mean, float standard_deviation)
    {
        // generate normalized random number by box muller method.
        // Normalized distribution is mean = 0.0. variance = 1.0.
        float x = Random.Range(0.0f, 1.0f);
        float y = Random.Range(0.0f, 1.0f);
        float random = Mathf.Sqrt(-2.0f * Mathf.Log(x)) * Mathf.Cos(2.0f * Mathf.PI * y);
        return random * standard_deviation + mean;
    }
}
