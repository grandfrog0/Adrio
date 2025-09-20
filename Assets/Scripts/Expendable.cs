using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Expendable : MonoBehaviour
{
    [SerializeField] private ParticleSystem particles;
    [SerializeField] private AudioSource au;
    [SerializeField] private float timer = 5;

    public void Play()
    {
        transform.parent = null;
        if (particles) particles.Play();
        if (au) au.Play();
        Invoke("Destroy_", timer);
    }

    private void Destroy_()
    {
        Destroy(gameObject);
    }
}
