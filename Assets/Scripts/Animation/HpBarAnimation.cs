using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HpBarAnimation : MonoBehaviour
{
    [SerializeField] private UIManager manager;
    [SerializeField] private Animator animator;

    public void PlayNarrow() //from gm
    {
        animator.Play("narrow_left");
    }

    public void PlayExpand() //from gm
    {
        animator.Play("expand_right");
    }

    public void OnAnimationEnd() //to gm
    {
        manager.OnHpBarAnimationEnd();
    }
}
