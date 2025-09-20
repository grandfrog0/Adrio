using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallItem : Item
{
    [SerializeField] Rigidbody2D rb;
    [SerializeField] CircleCollider2D circleCollider;
    [SerializeField] float strength = 10;
    private Transform parentTransform;
    private Movement parentMove;
    private float timer;

    public override void Use()
    {
        if (transform.parent != null)
        {
            picked_up = false;

            if (transform.parent && parent.TryGetComponent(out Movement move)) 
                parentMove = move;
            if (parentTransform == null) 
                parentTransform = transform.parent;
            transform.parent = null;
            
            rb.velocity = Vector2.zero;
            rb.AddForce(new Vector2(parentMove.move_input_axis * 0.25f, 1).normalized * strength, ForceMode2D.Impulse);
            circleCollider.enabled = true;
            timer = 2;
        }
    }

    public override void PickUp(BaseEntity entity = null)
    {
        base.PickUp(entity);
        
        rb.velocity = Vector2.zero;
        if (parentTransform) transform.parent = parentTransform;
        circleCollider.enabled = false;
    }

    private void FixedUpdate()
    {
        if (picked_up)
        {
            rb.velocity = Vector2.zero;
            transform.localPosition = Vector3.zero;
            transform.localRotation = Quaternion.identity;
            transform.localScale = Vector3.one;
        }

        if (timer > 0) timer -= Time.fixedDeltaTime;
    }

    private void OnTriggerStay2D(Collider2D coll)
    {
        if (parent && timer <= 0 && !picked_up && coll.gameObject.TryGetComponent(out BaseEntity entity) && entity == parent)
            PickUp();
    }
}
