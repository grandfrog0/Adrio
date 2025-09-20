using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SineTranslate : MonoBehaviour
{
    [SerializeField] Vector3 to;
    [SerializeField] float speed = 1;
    [SerializeField] bool isActive = true;
    private Vector3 startPos;
    private SineValue sine;

    public bool IsActive
    { 
        get { return isActive; }
        set { isActive = value; }
    }

    void FixedUpdate()
    {
        if (isActive)
            transform.localPosition = startPos + to * sine.Value;
    }

    private void Start()
    {
        startPos = transform.localPosition;
        sine = new SineValue(speed);
    }

    public void Reset()
    {
        transform.localPosition = startPos;
    }
}
