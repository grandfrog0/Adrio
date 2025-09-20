using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JennyMovement : Movement
{
    [SerializeField] private Transform wall_point;
    [SerializeField] private int have_wall;
    [SerializeField] private LayerMask wall_mask;
    [SerializeField] private BaseEntity entity;

    [SerializeField] private float start_gravityScale;
    [SerializeField] private float crawl_strength = 1f;

    private void FixedUpdate()
    {
        RaycastHit2D hit = Physics2D.Raycast(wall_point.position, transform.right * Mathf.Sign(mirror_in_axis.localScale.x), 0.2f, wall_mask);
        if (move_input_axis == 0 || hit.collider == null || (have_wall == 0 && super_charge_time < 1.5f) || super_charge_time <= 0)
        {
            have_wall = 0;
            is_super_using = false;
            charge_super = true;
        }
        else have_wall = (int) mirror_in_axis.localScale.x;
        Debug.DrawRay(wall_point.position, transform.right * Mathf.Sign(mirror_in_axis.localScale.x) * 0.2f, Color.yellow);
        
        if (have_wall != 0)
        {
            if (entity.IsLive())
            {
                if (move_input_axis == have_wall)
                {
                    charge_super = false;
                    super_charge_time -= Time.fixedDeltaTime;
                    is_super_using = true;

                    rb.velocity = new Vector2(rb.velocity.x, crawl_strength);

                    last_y = transform.position.y;
                }
            }
            else
            {
                have_wall = 0;
                is_super_using = false;
                charge_super = true;
            }
        }
    }
}
