using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FaceLookFor : MonoBehaviour
{
    [SerializeField] List<Transform> mimic;
    [SerializeField] List<float> values;
    [SerializeField] float speed;
    private List<Vector3> start_pos = new List<Vector3>();

    public FaceLookForType LookType { get; set; }
    public Transform target;
    public Rigidbody2D rb;

    private void FixedUpdate()
    {
        switch (LookType)
        {
            case FaceLookForType.Target:
                for(int i = 0; i < mimic.Count; i++)
                    mimic[i].localPosition = Vector3.Lerp(
                        mimic[i].localPosition, 
                        target ? (start_pos[i] + (target.position - mimic[i].position) / 50 * values[i]) : start_pos[i], 
                        speed * Time.fixedDeltaTime * 5
                    );
                break;

            case FaceLookForType.Velocity:
                for(int i = 0; i < mimic.Count; i++)
                    mimic[i].localPosition = Vector3.Lerp(
                        mimic[i].localPosition, 
                        rb ? (start_pos[i] + new Vector3(rb.velocity.normalized.x, rb.velocity.normalized.y) / 50 * values[i]) : start_pos[i], 
                        speed * Time.fixedDeltaTime * 5
                    );
                break;
        }
    }

    private void Awake()
    {
        foreach(Transform obj in mimic)
            start_pos.Add(obj.localPosition);
    }
}
