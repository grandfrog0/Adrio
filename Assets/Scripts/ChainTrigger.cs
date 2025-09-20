using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChainTrigger : MonoBehaviour
{
    public Rigidbody parent;

    void OnTriggerEnter2D(Collider2D coll)
    {
        parent.velocity = (parent.position - coll.gameObject.transform.position) * 2;
        parent.velocity = new Vector3(parent.velocity.x, parent.velocity.y, 0);
    }
}
