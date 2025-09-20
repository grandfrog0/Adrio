using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SineValue
{
    private float amplitude;
    private float speed;

    public float Value
    {
        get => Mathf.Sin(Time.time * speed) * amplitude;
    }

    public SineValue()
    {
        amplitude = 1;
        speed = 1;
    }

    public SineValue(float speed)
    {
        amplitude = 1;
        this.speed = speed;
    }

    public SineValue(float amplitude, float speed)
    {
        this.amplitude = amplitude;
        this.speed = speed;
    }
}
