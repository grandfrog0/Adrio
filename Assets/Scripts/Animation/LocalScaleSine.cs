using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LocalScaleSine : MonoBehaviour
{
    [SerializeField] float amplitude = 0.5f;
    [SerializeField] float speed = 1;
    [SerializeField] bool isActive = true;
    private Vector3 startScale;
    private SineValue sine;

    void FixedUpdate()
    {
        if (isActive)
            transform.localScale = new Vector3(startScale.x + sine.Value, startScale.y + sine.Value, 1);
    }

    void Start()
    {
        startScale = transform.localScale;
        sine = new SineValue(amplitude, speed);
    }
}
