using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationOnClick : MonoBehaviour
{
    public Animator anim;
    public string naming;
    public bool active = true;

    protected void OnMouseDown()
    {
        if (active) anim.Play(naming);
    }
}
