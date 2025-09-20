using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Knife : BaseProjectile
{
    public Vector2 to;
    public Animator anim;
    public Transform enemy;
    public float knife_speed = 20;
    public bool fired = false;
    Vector3 to1;
    public AudioSource warning_sound, fire_sound;

    public override void Fire(BaseEntity parent=null, float damage_strength=1, Vector2 to=new Vector2())
    {
        start_pos = transform.position;

        this.damage_strength = damage_strength;
        this.parent = parent;
        transform.rotation = Quaternion.AngleAxis(Mathf.Atan2(to.y, to.x) * Mathf.Rad2Deg, Vector3.forward);
        
        this.to = to;
        can_collision = false;
        anim.Play("ready");
        to1 = to;
        if (warning_sound) warning_sound.Play();
    }

    public void SetEnemy(Transform enemy)
    {
        this.enemy = enemy;
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();

        if (enemy && !fired)
        {
            // to1 = Vector3.Lerp(to1, enemy.position - transform.position, 2*Time.deltaTime);
            transform.rotation = Quaternion.AngleAxis(Mathf.Atan2(to1.y, to1.x) * Mathf.Rad2Deg, Vector3.forward);
            to = to1.normalized * knife_speed;
        }
    }

    public void Fire_()
    {
        rb.velocity = to;
        can_collision = true;
        anim.Play("fire");
        fired = true;
        if (fire_sound) fire_sound.Play();
    }
}
