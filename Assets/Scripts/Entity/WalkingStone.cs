using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WalkingStone : BaseEntity
{
    public Animator anim;
    public float left_border_x, right_border_x;

    [SerializeField] private bool switch_or_jump_if_wall;
    [SerializeField] private LayerMask wall_mask;
    private bool isByWall;

    public override void FixedUpdate()
    {
        base.FixedUpdate();
        
        if (transform.position.x <= left_border_x) movement.move_input_axis = 1;
        if (transform.position.x >= right_border_x) movement.move_input_axis = -1;
        anim.SetInteger("walk", (int) Mathf.Abs(movement.move_input_axis));

        RaycastHit2D hit = Physics2D.Raycast(transform.position, transform.right * Mathf.Sign(movement.move_input_axis), transform.localScale.x/1.5f, wall_mask);
        isByWall = hit.collider != null && hit.collider.gameObject != gameObject;
        Debug.DrawRay(transform.position, transform.right * Mathf.Sign(movement.move_input_axis) * transform.localScale.x/1.5f, Color.yellow);
        if (isByWall)
        {
            if (switch_or_jump_if_wall == false) movement.move_input_axis *= -1;
            else movement.Jump();
        }
    }

    private void OnCollisionEnter2D(Collision2D coll)
    {
        if (coll.gameObject.CompareTag("Player") && coll.gameObject.GetComponent<BaseEntity>())
        {
            GiveDamage(coll.gameObject.GetComponent<BaseEntity>());
        }
    }
}
