using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BigStone : BaseEntity
{
    [SerializeField] private Animator anim;

    [SerializeField] private FaceLookFor face_look_for;
    [SerializeField] private float see_range;
    [SerializeField] private float to_speed;

    [SerializeField] private bool isByWall;
    [SerializeField] private CapsuleCollider2D collider2d;
    [SerializeField] private LayerMask wall_mask;

    public override void FixedUpdate()
    {
        base.FixedUpdate();

        movement.SetMoveSpeed(Mathf.Lerp(movement.GetMoveSpeed(), to_speed, 0.2f));
        
        anim.SetInteger("walk", (int) Mathf.Abs(movement.move_input_axis));
        anim.SetFloat("speed", movement.GetMoveSpeed());

        if (movement.move_input_axis == 0)
        {
            face_look_for.LookType = FaceLookForType.Target;
            Collider2D[] colls = Physics2D.OverlapCircleAll(transform.position, see_range);
            foreach(Collider2D coll in colls)
            {
                Transform cur = null;
                if (coll.gameObject == this.gameObject) continue;
                if (coll.gameObject.GetComponent<BaseEntity>() && coll.gameObject.GetComponent<BaseEntity>().IsLive())
                {
                    if (cur == null || Vector3.Distance(transform.position, coll.gameObject.transform.position) < Vector3.Distance(transform.position, cur.position)) cur = coll.gameObject.transform;
                }
                face_look_for.target = cur;
            }
        }
        else face_look_for.LookType = FaceLookForType.Velocity;
        
        RaycastHit2D hit = Physics2D.Raycast(transform.position - transform.up * (collider2d.size + collider2d.offset).y/(1.75f / transform.localScale.y), transform.right * Mathf.Sign(movement.move_input_axis), (collider2d.size + collider2d.offset).x/1.75f, wall_mask);
        isByWall = hit.collider != null && hit.collider.gameObject != gameObject;
        Debug.DrawRay(transform.position - transform.up * (collider2d.size + collider2d.offset).y/(1.75f / transform.localScale.y), transform.right * Mathf.Sign(movement.move_input_axis) * (collider2d.size + collider2d.offset).x/1.75f, Color.yellow);
        if (isByWall) movement.Jump();
    }

    public float GetVelocitySpeed()
    {
        return Vector2.Distance(Vector2.zero, movement.rb.velocity);
    }

    private void OnCollisionEnter2D(Collision2D coll)
    {
        if (coll.gameObject.GetComponent<BaseEntity>())
        {
            GiveDamage(coll.gameObject.GetComponent<BaseEntity>());
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position, see_range);
    }

    public void RunWithSpeed(float value)
    {
        to_speed = value;
        movement.move_input_axis = 1;
    }
}
