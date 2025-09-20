using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Toggler : MonoBehaviour
{
    public Animator anim;
    public bool value = true;

    [SerializeField] UnityEvent onTurnedOn = new();
    [SerializeField] UnityEvent onTurnedOff = new();

    public void SetValue(bool val)
    {
        value = val;
        anim.Play(value ? "right" : "left");
        anim.SetBool("value", value);
    }

    public void Switch()
    {
        value = !value;
        anim.SetBool("value", value);

        if (value)
            onTurnedOn?.Invoke();
        else 
            onTurnedOff?.Invoke();
    }

    void Awake()
    {
        anim.Play(value ? "right" : "left");
        anim.SetBool("value", value);
    }
}
