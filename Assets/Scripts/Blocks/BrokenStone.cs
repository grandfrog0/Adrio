using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrokenStone : MonoBehaviour
{
    [SerializeField] Expendable child;
    [SerializeField] float impulseStrength = 3;

    private void OnCollisionEnter2D(Collision2D coll)
    {
        if (Tags.HasTag(coll.gameObject, "can_break_blocks"))
        {
            if (child) child.Play();
            
            if (coll.gameObject.TryGetComponent(out Movement move))
            {
                move.StopMove();
                move.Impulse(new Vector2(coll.gameObject.transform.position.x - transform.position.x, coll.gameObject.transform.position.y - transform.position.y) * impulseStrength);
            }

            Destroy(gameObject);
        }
    }
}
