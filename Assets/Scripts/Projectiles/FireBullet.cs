using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBullet : BaseProjectile
{
    [SerializeField] private Animator anim;

    public override void OnTriggerEnter2D(Collider2D coll)
    {
        // Debug.Log(coll.gameObject.name);
        if (coll.gameObject.GetComponent<BaseEntity>() && coll.gameObject.GetComponent<BaseEntity>().GetEntityType() == Types.EntityTypes.Firebird) return;
        base.OnTriggerEnter2D(coll);
    }

    public override void Destroy_()
    {
        anim.Play("fade");
    }
}
