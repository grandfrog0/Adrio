using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirebirdGrabObject : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private byte grab_state = 0;
    [SerializeField] private Movement move;
    [SerializeField] private Transform grab_point;

    [SerializeField] private BaseEntity kill_object_on_grab;
    [SerializeField] private Movement target_move;
    [SerializeField] private BaseEntity this_entity;
    [SerializeField] private float invoke_time;

    public void GoGrab()
    {
        grab_state = 1;
    }

    void OnGrab()
    {
        target_move.enabled = false;
        if (kill_object_on_grab) Invoke("kill", invoke_time);
    }

    void kill()
    {
        if (kill_object_on_grab) kill_object_on_grab.Kill(this_entity);
    }

    void FixedUpdate()
    {
        if (grab_state == 1)
        {
            move.SetInputAxis(new Vector2(target.position.x - transform.position.x, target.position.y - transform.position.y).normalized);
            if (Vector3.Distance(target.position, transform.position) < 0.5f)
            {
                grab_state = 2;
                target.parent = grab_point;
                target.gameObject.layer = LayerMask.NameToLayer("IgnoreAll");
                OnGrab();
            }
        }
        if (grab_state == 2)
        {
            move.SetInputAxis(Vector2.up);
            if (target.parent == grab_point)
            {
                target.localPosition = Vector3.zero;
                target.localRotation = Quaternion.identity;
                target.localScale = new Vector3(Mathf.Abs(target.localScale.x), target.localScale.y, 1);
            }
        }
    }
}
