using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomRotation : MonoBehaviour
{
    [SerializeField] float step;
    
    void Start()
    {
        transform.localRotation = Quaternion.Euler(0, 0, MyMath.Int(Random.Range(0f, 360f) / step) * step);
    }
}
