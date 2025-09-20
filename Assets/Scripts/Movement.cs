using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    public Rigidbody2D rb;
    [SerializeField] private BaseEntity base_entity;
    public Transform mirror_in_axis, legs_point;
    private Vector3 mia_start;
    public bool mirror_by_axis = true;
    public AudioSource steps_sound, jump_sound;
    [SerializeField] private float steps_sound_timer;
    public bool can_move = true;
    public bool can_fly;

    public float speed_default;
    public float jump_strength_default;

    public float move_input_axis;
    public float move_input_axis_vertical;
    public bool grounded;
    private bool grounded_ray, grounded_coll;
    protected float last_y = -999;
    [SerializeField] private float ground_distance = 0.25f;
    [SerializeField] float land_distance = 6;
    [SerializeField] private ParticleSystem on_landed_particles, jumped_particles;
    [SerializeField] private LayerMask ground_mask;

    public enum SuperType {Nothing, Dash, Another};
    public float super_max_time;
    public float super_charge_time;
    public bool is_super_using;
    public bool charge_super = true;
    public SuperType super_type;
    public List<KeyCode> super_keycodes;
    public bool keycode_down;

    private void Update()
    {
        if (!base_entity.IsLive()) StopMove(true);
        
        if (can_move) rb.velocity = new Vector2(move_input_axis*speed_default, rb.velocity.y);
        rb.velocity = new Vector2(rb.velocity.x, Mathf.Clamp(rb.velocity.y, -45f, 500f));
        if (can_fly && can_move) rb.velocity = new Vector2(rb.velocity.x, move_input_axis_vertical*speed_default);
        if (mirror_by_axis)
        {
            if (move_input_axis > 0) mirror_in_axis.localScale = mia_start;
            if (move_input_axis < 0) mirror_in_axis.localScale = new Vector3(-mia_start.x, mia_start.y, mia_start.z);
        }
        mirror_in_axis.localScale = new Vector3(mirror_in_axis.localScale.x, Mathf.Clamp(mia_start.y + rb.velocity.y/200, 0.75f, 1.25f), 1);
        if (steps_sound != null)
        {
            if (steps_sound_timer <= 0)
            {
                if (IsWalk() != 0 && grounded)
                {
                    steps_sound.Play();
                    steps_sound_timer = 0.4f;
                }
                
            }
            else steps_sound_timer -= Time.deltaTime;
        }
        
        RaycastHit2D hit = Physics2D.Raycast(legs_point.position, transform.up * -1, ground_distance, ground_mask);
        grounded_ray = hit.collider != null && hit.collider.gameObject != gameObject;
        grounded = grounded_ray || grounded_coll;
        if (grounded)
        {
            if (HasCritialFallDistance()) OnLanded();
            last_y = transform.position.y;
        }
        else
        {
            if (last_y < transform.position.y) last_y = transform.position.y;
        }

        if (super_charge_time < super_max_time && charge_super) super_charge_time += Time.deltaTime;
        if (super_charge_time > super_max_time) super_charge_time = super_max_time;
    }

    public bool HasCritialFallDistance()
    {
        return (last_y - transform.position.y > land_distance);
    }

    public void SetInputAxis(Vector2 vector)
    {
        move_input_axis = vector.x;
        move_input_axis_vertical = vector.y;
    }

    public Vector2 GetInputAxis()
    {
        return new Vector2(move_input_axis, move_input_axis_vertical);
    }

    private void OnLanded()
    {
        if (!base_entity.IsLive()) return;
        if (on_landed_particles) on_landed_particles.Play();
    }

    public void StopMove(bool forever=false)
    {
        if (!can_move) return;
        can_move = false;
        if (!forever) Invoke("StartMove", 0.2f);
    }

    public void StartMove()
    {
        can_move = true;
    }

    public void Impulse(Vector2 vector)
    {
        rb.AddForce(vector, ForceMode2D.Impulse);
    }

    public virtual void Jump()
    {
        if (!grounded || !can_move || !this.enabled) return;
        rb.velocity = new Vector2(rb.velocity.x, 0);
        rb.AddForce(transform.up*jump_strength_default, ForceMode2D.Impulse);
        if (jump_sound != null) jump_sound.Play();
        if (jumped_particles) jumped_particles.Play();
    }

    public int IsWalk()
    {
        if (rb.velocity.x != 0 && move_input_axis != 0) return 1;
        return 0;
    }

    public float GetMoveSpeed()
    {
        return speed_default;
    }

    public void SetMoveSpeed(float value)
    {
        speed_default = value;
    }

    public void SetJumpStrength(float value)
    {
        jump_strength_default = value;
    }

    public void SetSuperType(int value)
    {
        switch (value)
        {
            case 0:
                super_type = SuperType.Nothing;
                break;

            case 1:
                super_type = SuperType.Dash;
                break;

            case 2:
                super_type = SuperType.Another;
                break;
        }
    }

    public virtual void TryToSuper()
    {
        if (super_charge_time == super_max_time && super_type != SuperType.Nothing) 
        {
            UseSuper(super_type);
        }
    }

    public virtual void UseSuper(SuperType type)
    {
        switch (type)
        {
            case SuperType.Dash:
                if (!grounded)
                {
                    StopMove();
                    rb.velocity = new Vector2(rb.velocity.x, 0);
                    Impulse(new Vector2(rb.velocity.x, Mathf.Clamp(rb.velocity.y, 1000, 1)).normalized*jump_strength_default);
                    
                    super_charge_time = 0;
                }
                break;
        }
    }

    public float GetSuperChargedProcent()
    {
        if (super_max_time == 0) return 0;
        return super_charge_time/super_max_time;
    }

    public bool IsSuperUsing() {return is_super_using;}

    public virtual void OnCollisionStay2D(Collision2D coll)
    {
        foreach (ContactPoint2D point in coll.contacts) 
        {
            if (point.normal.y == 1f && coll.gameObject != gameObject)
            {
                grounded_coll = true;
                return;
            }
        }
        grounded_coll = false;
    }

    public virtual void OnCollisionExit2D(Collision2D coll)
    {
        foreach (ContactPoint2D point in coll.contacts) 
        {
            if (point.normal.y == 1f && coll.gameObject != gameObject)
            {
                grounded = true;
                return;
            }
        }
        grounded_coll = false;
    }

    public virtual void Awake()
    {
        if (mirror_in_axis) mia_start = mirror_in_axis.localScale;
    }
}
