using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationSimpleCommands : MonoBehaviour
{
    [SerializeField] private ParticleSystem particles;
    [SerializeField] private AudioSource au;
    [SerializeField] private GameObject obj;
    [SerializeField] private MonoBehaviour method_obj;
    [SerializeField] private string method;

    public void SendMethod()
    {
        method_obj.SendMessage(method);
    }

    public void Disable()
    {
        gameObject.SetActive(false);
    }

    public void Destroy_()
    {
        Destroy(obj);
    }

    public void Particles()
    {
        particles.Play();
    }

    public void AudioPlay()
    {
        au.Play();
    }
}
