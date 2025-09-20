using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sword : BaseProjectile
{
    public Animator anim;
    public bool fired = false;
    public AudioSource warning_sound, fire_sound;
    public float range = 5;

    public override void Fire(BaseEntity parent=null, float damage_strength=1, Vector2 to=new Vector2())
    {
        start_pos = transform.position;

        this.damage_strength = damage_strength;
        this.parent = parent;
        transform.rotation = Quaternion.AngleAxis(Mathf.Atan2(to.y, to.x) * Mathf.Rad2Deg, Vector3.forward);
        
        can_collision = false;
        anim.Play("ready");
        if (warning_sound) warning_sound.Play();
    }

    public void Fire_()
    {
        anim.Play("fire");
        fired = true;
        if (fire_sound) fire_sound.Play();

        Collider2D[] colls = Physics2D.OverlapCircleAll(transform.position, range);
        foreach(Collider2D coll in colls)
        {
            if ((parent == null || coll.gameObject != parent.gameObject) && ((enemy_mask.value & (1 << coll.gameObject.layer)) != 0) && coll.gameObject.GetComponent<BaseEntity>())
            {
                coll.gameObject.GetComponent<BaseEntity>().TakeDamage(damage_strength, parent, transform);
            }
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position, range);
    }
}
