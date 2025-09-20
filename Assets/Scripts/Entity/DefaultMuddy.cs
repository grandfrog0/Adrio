using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefaultMuddy : BaseEntity
{
    [SerializeField] private Animator anim;
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private float see_range;
    [SerializeField] private float go_up_multiplier = 2;
    [SerializeField] private bool hidden = true;
    [SerializeField] private bool have_grass;
    [SerializeField] private SpriteRenderer no_grass, with_grass, grass;
    [SerializeField] private Vector3 start_pos;
    [SerializeField] private FaceLookFor face_look_for;
    [SerializeField] private float catching_timer = 0;

    [SerializeField] private AudioSource wake_up_sound;

    public override void FixedUpdate()
    {
        base.FixedUpdate();
        
        no_grass.enabled = !have_grass;
        with_grass.enabled = have_grass;
        grass.enabled = have_grass;

        if (hidden)
        {
            if (catching_timer <= 0) 
            {
                catching_timer = Random.Range(4, 20);
                anim.Play("catching");
            }
            else catching_timer -= Time.fixedDeltaTime;
        }

        Collider2D[] colls = Physics2D.OverlapCircleAll(start_pos, see_range);
        foreach(Collider2D coll in colls)
        {
            if (coll.gameObject.CompareTag("Player") || (coll.gameObject.GetComponent<BaseEntity>() && coll.gameObject.GetComponent<BaseEntity>().GetEntityType() == Types.EntityTypes.BigStone))
            {
                if (hidden) WakeUp();
                else
                {
                    face_look_for.target = coll.gameObject.transform; 
                    break;
                }
            }
            else if (!hidden) face_look_for.target = null;
        }

        rb.velocity = Vector2.Lerp(rb.velocity, Vector2.zero, 3*Time.deltaTime);
        transform.position = Vector3.Lerp(transform.position, start_pos, 5*Time.deltaTime);
    }

    private void WakeUp()
    {
        hidden = false;
        anim.SetBool("waked_up", true);
        // anim.Play("wake_up");
        Invoke("WakeUpStep2", 0.4f);
        if (wake_up_sound) wake_up_sound.Play();
    }

    private void WakeUpStep2()
    {
        start_pos += Vector3.up*go_up_multiplier;
    }

    private void GoSleep()
    {
        anim.Play("died");
        Invoke("GoSleep2", 0.4f);
        if (hurt_sound) hurt_sound.Play();
    }

    private void GoSleep2()
    {
        start_pos -= Vector3.up*go_up_multiplier;
    }

    public override void Awake()
    {
        base.Awake();

        start_pos = transform.position;
    }

    private void OnCollisionEnter2D(Collision2D coll)
    {
        if (coll.gameObject.CompareTag("Player") && coll.gameObject.GetComponent<BaseEntity>())
        {
            foreach (ContactPoint2D point in coll.contacts) 
            {
                if (point.normal.y >= 0f) GiveDamage(coll.gameObject.GetComponent<BaseEntity>());
            }
        }
    }
    
    void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position, see_range);
    }

    public override void NormalizeParameters()
    {
        if (!IsLive())
        {
            if (!hidden)
            {
                GoSleep();
            }
            hidden = false;
        }
    }
}
