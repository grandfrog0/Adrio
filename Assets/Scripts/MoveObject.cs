using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveObject : MonoBehaviour
{
    [SerializeField] private List<Vector3> points;
    [SerializeField] private int cur_point;
    [SerializeField] private int last_point;
    [SerializeField] private float time_start;
    [SerializeField] private bool auto_move = false;

    [SerializeField] private float movement_time;

    public void SetPoint(int value)
    {
        last_point = cur_point;
        cur_point = value;
        time_start = Time.time;
    }

    public void NextPoint() 
    {
        SetPoint((cur_point + 1) % points.Count);
    }

    public void BackPoint() 
    {
        if (cur_point <= 0) SetPoint(points.Count - 1);
        else SetPoint(cur_point - 1);
    }

    private void Update()
    {
        if (!auto_move)
        {
            if (Time.time <= time_start + movement_time)
            {
                transform.position = Vector3.Lerp(points[cur_point], points[last_point], (time_start + movement_time - Time.time));
            }
            else
            {
                transform.position = points[cur_point];
            }
        }
        else
        {
            if (Vector3.Distance(transform.localPosition, points[last_point]) > 0.1f)
            {
                transform.localPosition = Vector3.Lerp(transform.localPosition, points[last_point], movement_time * Time.deltaTime);
            }
            else
            {
                transform.localPosition = points[last_point];
                NextPoint();
            }
        }
    }
}
