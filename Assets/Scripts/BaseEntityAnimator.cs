using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseEntityAnimator : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private Movement movement;
    [SerializeField] private BaseEntity base_entity;
    [SerializeField] private Collider2D coll;
    [SerializeField] private List<SpriteRenderer> renderers;
    [SerializeField] private List<Item> items = new List<Item>();
    [SerializeField] private bool have_died_animation, have_walk_animation;
    [SerializeField] private string super_animation_name;

    [SerializeField] private bool is_active = true;
    [SerializeField] private int layers_delta = 2;
    [SerializeField] private List<int> start_layers;

    [SerializeField] private bool has_jump_animation;

    [SerializeField] private bool can_sit_down;
    [SerializeField] private string sit_animation;
    [SerializeField] private Vector2 sit_collider_size_start, sit_collider_offset_start;
    [SerializeField] private Vector2 sit_collider_size, sit_collider_offset;
    [SerializeField] private bool is_sitting;
    [SerializeField] private CapsuleCollider2D coll_c;

    [SerializeField] private bool disable_gameobject_on_start;

    private void FixedUpdate()
    {
        if (have_walk_animation && !movement.IsSuperUsing())
        {
            animator.SetInteger("walk", movement.IsWalk());
        }
        if (super_animation_name != "") // && !animator.GetCurrentAnimatorStateInfo(0).IsName(super_animation_name))
        {
            animator.SetBool(super_animation_name, movement.IsSuperUsing());
        }
        if (!base_entity.IsLive())
        {
            if (have_died_animation) animator.Play("died");
            movement.move_input_axis = 0;
            movement.move_input_axis_vertical = 0;
            if (coll.enabled)
            {
                coll.enabled = false;
                foreach(SpriteRenderer renderer in renderers)
                {
                    renderer.sortingOrder += 50;
                    renderer.sortingLayerName = "UI";
                }
                foreach(Item item in items)
                {
                    if (item) item.SetSortingLayer(50, "UI");
                }
            }
        }
        else
        {
            if (has_jump_animation)
            {
                animator.SetBool("jump", movement.rb.velocity.y > 2);
            }
            if (can_sit_down)
            {
                if (movement.move_input_axis_vertical < 0 && movement.grounded && movement.rb.velocity.x == 0)
                {
                    animator.Play(sit_animation);
                    is_sitting = true;

                    coll_c.size = sit_collider_size;
                    coll_c.offset = sit_collider_offset;
                }
                else if (is_sitting)
                {
                    animator.Play("idle");
                    is_sitting = false;

                    coll_c.size = sit_collider_size_start;
                    coll_c.offset = sit_collider_offset_start;
                }
            }
        }

        // for(int i = 0; i < items.Count; i++)
        // {
        //     if (items[i] == null) continue;

        //     if (base_entity.items.IndexOf(items[i]) == -1)
        //     {
        //         items[i].SetSortingOrder(0);
        //     }
        // }
        foreach(Item item in base_entity.items) if (items.IndexOf(item) == -1) items.Add(item);
    }

    public void SetAnimatorSpeed(float speed)
    {
        animator.speed = speed;
    }

    public void SetEntityActive(bool value)
    {
        if (is_active == value) return;
        
        is_active = value;
        if (is_active)
        {
            gameObject.layer = LayerMask.NameToLayer("Player");
        }
        else
        {
            gameObject.layer = LayerMask.NameToLayer("IgnoreEntities");
        }
    }

    public void SetLayer(int multiplier, bool load_items=false)
    {
        for(int i = 0; i < renderers.Count; i++)
        {
            renderers[i].sortingOrder = start_layers[i] + layers_delta * multiplier;
        }

        if (load_items)foreach(Item item in base_entity.items) if (items.IndexOf(item) == -1) items.Add(item);

        foreach(Item item in items) if (item) item.SetSortingOrder(layers_delta * multiplier);
    }

    private void Awake()
    {
        if (can_sit_down)
        {
            sit_collider_size_start = coll_c.size;
            sit_collider_offset_start = coll_c.offset;
        }
        start_layers.Clear();
        foreach(SpriteRenderer renderer in renderers)
        {
            start_layers.Add(renderer.sortingOrder);
        }
    }

    private void Start()
    {
        if (disable_gameobject_on_start) gameObject.SetActive(false);
        foreach(Item item in base_entity.items) if (items.IndexOf(item) == -1) items.Add(item);
    }
}
