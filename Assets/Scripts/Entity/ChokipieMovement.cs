using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChokipieMovement : Movement
{
    [SerializeField] private Transform roof_point;
    [SerializeField] private bool have_roof;
    [SerializeField] private LayerMask roof_mask;
    [SerializeField] private float roof_multiplier = 0.5f;
    [SerializeField] private CapsuleCollider2D caps_coll;
    [SerializeField] private Vector2 coll_size_start, coll_size, coll_offset_start, coll_offset;
    [SerializeField] private bool is_small;
    [SerializeField] private Animator anim;
    [SerializeField] private float speed_big;
    [SerializeField] private float speed_small_full = 1;
    [SerializeField] private float speed_small_empty = 1;
    [SerializeField] private float speed_small_now = 5;
    [SerializeField] private float super_decrease_speed = 1;

    private void FixedUpdate()
    {
        RaycastHit2D hit = Physics2D.Raycast(roof_point.position, transform.up * Mathf.Sign(mirror_in_axis.localScale.y), roof_multiplier, roof_mask);
        have_roof = hit.collider != null;
        Debug.DrawRay(roof_point.position, transform.up * Mathf.Sign(mirror_in_axis.localScale.y) * roof_multiplier, Color.yellow);

        if (move_input_axis_vertical == -1)
        {
            if (!is_small)
            {
                is_small = true;
                caps_coll.size = coll_size;
                caps_coll.offset = coll_offset;
                anim.SetTrigger("to_mini");
                charge_super = false;
            }
            speed_small_now = Mathf.Lerp(speed_small_now, speed_small_empty + (speed_small_full - speed_small_empty)*(super_charge_time/super_max_time), 3*Time.deltaTime);
            speed_default = speed_small_now;
        }
        else
        {
            if (!have_roof)
            {
                if (is_small)
                {
                    is_small = false;
                    caps_coll.size = coll_size_start;
                    caps_coll.offset = coll_offset_start;
                    anim.SetTrigger("to_big");
                }
            }
            if (!is_small)
            {
                speed_default = speed_big;
            }
        }
        anim.SetBool("is_small", is_small);
        anim.SetInteger("walk", IsWalk());
        
        if (is_small && IsWalk() == 1 && super_charge_time > 0)
        {
            super_charge_time -= Time.fixedDeltaTime * super_decrease_speed;
            is_super_using = true;
        }
        if (!is_small)
        {
            charge_super = true;
            is_super_using = false;
        }
    }

    public override void Awake()
    {
        base.Awake();
        coll_size_start = caps_coll.size;
        coll_offset_start = caps_coll.offset;
        speed_big = speed_default;
    }
}
