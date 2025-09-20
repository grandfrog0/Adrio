using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Firebird : BaseEntity
{
    [SerializeField] private Transform target;
    [SerializeField] private Vector3 chill_point;
    [SerializeField] private Animator anim;
    [SerializeField] private float dist_in = 7;
    [SerializeField] private float dist_out = 4;
    [SerializeField] private Transform right_point;
    [SerializeField] private LayerMask wall_mask;
    [SerializeField] private bool is_by_wall;
    [SerializeField] private Vector3 add_vector;
    [SerializeField] private float see_range;
    [SerializeField] private GameObject fire_bullet_prefab;
    [SerializeField] private Transform head_transform;
    [SerializeField] private float fire_strength = 5;
    [SerializeField] private float time_min, time_max;
    [SerializeField] private List<Transform> ignore_as_target;
    [SerializeField] private Vector3 add_vector_range;
    [SerializeField] private bool see_target;
    [SerializeField] private bool target_was_in_view;
    [SerializeField] private bool has_chill_point=true;
    private float timer = 0;

    public override void FixedUpdate()
    {
        base.FixedUpdate();

        add_vector = Vector2.zero;
        RaycastHit2D hit = Physics2D.Raycast(right_point.position, Vector2.right * movement.move_input_axis * 1, 1f, wall_mask);
        is_by_wall = hit.collider != null && hit.collider.gameObject != gameObject;
        if (is_by_wall && movement.move_input_axis != 0) add_vector = Vector2.up;
        if (movement.grounded && target_was_in_view && see_target) add_vector = Vector2.up;
        if (target_was_in_view && see_target && transform.position.y < target.position.y) add_vector = Vector2.up;

        if (target)
        {
            if (see_target)
            {
                float distance = Vector3.Distance(transform.position, target.position);
                if (distance > dist_in) movement.SetInputAxis(((target.position - transform.position).normalized + add_vector).normalized);
                else if (distance < dist_out) movement.SetInputAxis((-(target.position - transform.position).normalized + add_vector).normalized);
                else movement.SetInputAxis(Vector2.Lerp(movement.GetInputAxis(), add_vector, Time.fixedDeltaTime * 5));

                movement.mirror_by_axis = false;
                movement.mirror_in_axis.localScale = new Vector3(Mathf.Sign((target.position - transform.position).x), 1, 1);
                right_point.localPosition = new Vector3(Mathf.Abs(right_point.localPosition.x) * Mathf.Sign(movement.move_input_axis * (target.position - transform.position).x), right_point.localPosition.y);
            }
            else if (target_was_in_view) movement.SetInputAxis(Vector2.Lerp(movement.GetInputAxis(), Vector2.zero, Time.fixedDeltaTime * 5));
            else if (has_chill_point) FlyBack();
        }
        else
        {
            if (has_chill_point) FlyBack();
        }

        anim.SetBool("fly", (Vector2.Distance(movement.rb.velocity, Vector2.zero) > 1) || !movement.grounded);

        movement.can_fly = movement.GetInputAxis() != Vector2.zero;
        if ((Vector2.Distance(movement.rb.velocity, Vector2.zero) > 1) || !movement.grounded)
        {
            movement.rb.gravityScale = 0;
        }
        else
        {
            movement.rb.gravityScale = 2;
        }

        if (target && Vector3.Distance(chill_point + add_vector_range, target.position) > see_range)
        {
            target = null;
            target_was_in_view = false;
        }
        if (target == null)
        {
            Collider2D[] colls = Physics2D.OverlapCircleAll(chill_point + add_vector_range, see_range);
            foreach(Collider2D coll in colls)
            {
                if (coll.gameObject.CompareTag("Player") && coll.gameObject != gameObject && ignore_as_target.IndexOf(coll.gameObject.transform) == -1)
                {
                    target = coll.gameObject.transform;
                    break;
                }
            }
        }
        if (target)
        {
            RaycastHit2D hit1 = Physics2D.Raycast(transform.position + (target.position - transform.position).normalized*transform.localScale.x, (target.position - transform.position).normalized, Vector3.Distance(transform.position + (target.position - transform.position).normalized*transform.localScale.x, target.position), wall_mask);
            see_target = (hit1.collider == null || hit1.collider.gameObject.transform == target);
            // if (hit1.collider) Debug.Log(hit1.collider.gameObject.name);
            if (see_target) target_was_in_view = true;
            Debug.DrawRay(transform.position + (target.position - transform.position).normalized*transform.localScale.x, (target.position - transform.position).normalized * (Vector3.Distance(transform.position + (target.position - transform.position).normalized*transform.localScale.x, target.position)), Color.green);

            if (see_target)
            {
                if (timer <= 0) 
                {
                    timer = Random.Range(time_min, time_max);
                    Fire();
                }
                else timer -= Time.fixedDeltaTime;
            }
        }
    }

    private void FlyBack()
    {
        if (Vector3.Distance(chill_point, transform.position) > 1f) movement.SetInputAxis(((chill_point - transform.position).normalized + add_vector).normalized);
        else movement.SetInputAxis(Vector2.Lerp(movement.GetInputAxis(), add_vector, Time.fixedDeltaTime * 5));

        movement.mirror_by_axis = true;
        right_point.localPosition = new Vector3(Mathf.Abs(right_point.localPosition.x), right_point.localPosition.y);
    }

    public void Fire()
    {
        BaseProjectile obj = Instantiate(fire_bullet_prefab, head_transform.position, Quaternion.identity).GetComponent<BaseProjectile>();
        obj.Fire(this, GetDamage(), ((target.position - transform.position).normalized + new Vector3(Random.Range(-0.1f, 0.1f), Random.Range(-0.1f, 0.1f))).normalized * fire_strength);
    }

    public override void Awake()
    {
        base.Awake();

        chill_point = transform.position;
        timer = Random.Range(time_min, time_max);
    }
    
    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position + add_vector_range, see_range);
    }
}
