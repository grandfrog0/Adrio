using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseProjectile : MonoBehaviour
{
    [Header("Start Parameters")]
    public float damage_strength = 1;
    public bool destroy_after_touch;
    public bool can_touch_obstacles;
    public bool can_collision = true;
    public float destroy_distance;
    [Header("In-Game Parameters")]
    public BaseEntity parent;
    public Vector3 start_pos;
    [Header("Base")]
    public Rigidbody2D rb;
    public LayerMask enemy_mask, obstacles_mask;

    public virtual void Fire(BaseEntity parent=null, float damage_strength=1, Vector2 to=new Vector2())
    {
        start_pos = transform.position;

        rb.velocity = to;
        this.damage_strength = damage_strength;
        this.parent = parent;
        transform.rotation = Quaternion.AngleAxis(Mathf.Atan2(to.y, to.x) * Mathf.Rad2Deg, Vector3.forward);
    }

    public virtual void OnTriggerEnter2D(Collider2D coll)
    {
        if (!parent || coll.gameObject == parent.gameObject || !can_collision) return;

        if (((enemy_mask.value & (1 << coll.gameObject.layer)) != 0) && coll.gameObject.GetComponent<BaseEntity>())
        {
            can_collision = false;
            coll.gameObject.GetComponent<BaseEntity>().TakeDamage(damage_strength, parent, transform);
            if (destroy_after_touch) Destroy_();
            Debug.Log(1);
        }
        if (((obstacles_mask.value & (1 << coll.gameObject.layer)) != 0) && can_touch_obstacles)
        {
            can_collision = false;
            rb.velocity = Vector2.zero;
            if (destroy_after_touch) Destroy_();
        }
    }

    public virtual void Destroy_()
    {
        Destroy(gameObject);
    }

    public virtual void FixedUpdate()
    {
        if (destroy_distance > 0 && Vector3.Distance(start_pos, transform.position) > destroy_distance) Destroy_();
    }
}
