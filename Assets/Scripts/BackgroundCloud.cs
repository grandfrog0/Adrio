using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundCloud : MonoBehaviour
{
    public float parallax_strength = 1f;
    private Transform cam;
    private Vector3 start_position;

    private void FixedUpdate()
    {
        transform.position = new Vector3(start_position.x + cam.position.x/parallax_strength, start_position.y + cam.position.y, 0);
        if (transform.localPosition.x < -20) start_position.x += 35;
        if (transform.localPosition.x > 20) start_position.x += -35;
    }

    private void Awake()
    {
        cam = Camera.main.gameObject.transform;
        start_position = transform.localPosition;
    }
}
