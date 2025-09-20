using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraWatch : MonoBehaviour
{
    [SerializeField] private List<Transform> targets;
    [SerializeField] private Vector3 add_vector;
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private float cam_speed;
    public float min_distance;
    [SerializeField] private float clamp_left = 1.75f;
    [SerializeField] private float clamp_right = 1000f;
    [SerializeField] private float clamp_bottom = 1;
    [SerializeField] private float clamp_up = 1000f;
    [SerializeField] private float cam_size = 6;
    [SerializeField] private float size_speed = 2;
    private Camera cam;
    [SerializeField] private float shake_time;
    [SerializeField] private float shake_strength = 0.5f;
    [SerializeField] private List<Transform> targets_in_buffer;

    private void Update()
    {
        if (targets.Count == 0) return;
    
        transform.position = Vector3.Lerp(transform.position, AverageVector3() + add_vector + new Vector3(rb.velocity.x, rb.velocity.y).normalized*0.1f, cam_speed * Time.deltaTime);
        transform.position = new Vector3(Mathf.Clamp(transform.position.x, clamp_left, clamp_right), Mathf.Clamp(transform.position.y, clamp_bottom, clamp_up), transform.position.z);
        if (shake_time > 0)
        {
            transform.position += new Vector3(Random.Range(-shake_strength, shake_strength), Random.Range(-shake_strength, shake_strength));
            shake_time -= Time.deltaTime;
        }

        if (cam) cam.orthographicSize = Mathf.Lerp(cam.orthographicSize, cam_size, size_speed*Time.deltaTime);
    }

    public void Shake(float time=0.25f)
    {
        if (time == 0)
        {
            shake_time = 0.25f;
            return;
        }
        else
        {
            shake_time = time;
        }
        // shake_strength = strength;
    }

    public void ToEnd()
    {
        if (targets.Count == 0) return;

        transform.position = AverageVector3() + add_vector + new Vector3(rb.velocity.x, rb.velocity.y).normalized*0.1f;
        transform.position = new Vector3(Mathf.Clamp(transform.position.x, clamp_left, clamp_right), Mathf.Clamp(transform.position.y, clamp_bottom, clamp_up), transform.position.z);
    }

    public Vector3 GetAddVector()
    {
        return add_vector;
    }

    private Vector3 AverageVector3()
    {
        if (targets.Count == 0) return new Vector3();
        Vector3 vector3 = new Vector3();
        foreach(Transform target in targets)
        {
            vector3 += target.position;
        }
        return vector3 / targets.Count;
    }

    public void AddTarget(Transform target)
    {
        targets.Add(target);
    }

    public void AddTargetToBuffer(Transform target)
    {
        targets_in_buffer.Add(target);
    }

    public void AddTargetFromBuffer()
    {
        if (targets_in_buffer.Count == 0) return;
        targets.Add(targets_in_buffer[0]);
        targets_in_buffer.RemoveAt(0);
    }
    
    public void AddTargetAfterTime(float value)
    {
        Invoke("AddTargetFromBuffer", value);
    }

    public void RemoveTarget(Transform target)
    {
        targets.Remove(target);
    }

    public void RemoveLastTarget()
    {
        if (targets.Count == 0) return;
        targets.RemoveAt(targets.Count - 1);
    }

    public void RemoveLastTargetAfterTime(float value)
    {
        Invoke("RemoveLastTarget", value);
    }

    public bool HasTarget(Transform target)
    {
        if (targets.IndexOf(target) == -1) return false;
        return true;
    }

    public float GetSize()
    {
        return cam_size;
    }

    public void SetSize(float size)
    {
        cam_size = size;
    }

    public void SetRb(Rigidbody2D value)
    {
        rb = value;
    }

    public void SetClampLeft(float value)
    {
        clamp_left = value;
    }

    public void SetClampRight(float value)
    {
        clamp_right = value;
    }

    public void SetClampUp(float value)
    {
        clamp_up = value;
    }

    public void SetClampBottom(float value)
    {
        clamp_bottom = value;
    }

    private void Awake()
    {
        cam = GetComponent<Camera>();
    }
}
