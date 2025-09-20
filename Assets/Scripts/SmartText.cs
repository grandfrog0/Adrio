using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmartText : MonoBehaviour
{
    enum SmartTextType {Default, Fade, Move, TempMove};

    [SerializeField] private SmartTextType type = SmartTextType.Default;
    [SerializeField] private float range;
    [SerializeField] private float speed;
    [SerializeField] private float value;
    [SerializeField] private float value2; //additional value
    [SerializeField] private List<Transform> targets;
    [SerializeField] private Transform follow_transform;
    private TMPro.TMP_Text text;
    private Color start_color;
    private RectTransform rect;
    private Vector3 start_position;
    [SerializeField] Vector3 comparative_position;
    private Vector3 start_anchoredPosition;
    [SerializeField] private bool target_in_range;

    private void FixedUpdate()
    {
        foreach(Transform target in targets)
        {
            if (Vector3.Distance(target.position, start_position + comparative_position) < range)
            {
                target_in_range = true;
                return;
            }
        }
        target_in_range = false;
    }

    private void Update()
    {
        if (follow_transform) comparative_position = follow_transform.position;
        else comparative_position = Vector3.zero;

        switch (type)
        {
            case SmartTextType.Fade:
                transform.position = comparative_position + start_position;

                if (target_in_range) text.color = Color.Lerp(text.color, start_color, speed*Time.deltaTime);
                else text.color = Color.Lerp(text.color, Color.clear, speed*2*Time.deltaTime);
                break;

            case SmartTextType.Move:
                if (target_in_range) transform.position = Vector3.Lerp(transform.position, comparative_position + start_position + Vector3.right*value, speed*Time.deltaTime);
                else transform.position = Vector3.Lerp(transform.position, comparative_position + start_position, speed*Time.deltaTime);
                break;
            
            case SmartTextType.TempMove:
                if (value2 > 0) value2 -= Time.deltaTime;
                else if (range > 0)
                {
                    range -= Time.deltaTime;
                    rect.anchoredPosition = Vector3.Lerp(rect.anchoredPosition, start_anchoredPosition + Vector3.right*value, speed*Time.deltaTime);
                }
                else rect.anchoredPosition = Vector3.Lerp(rect.anchoredPosition, start_anchoredPosition, speed*Time.deltaTime);
                break;
        }
    }

    public void SetRange(float value)
    {
        range = value;
    }

    public void GoToTransform(Transform tr)
    {
        start_position = tr.position;
        transform.position = comparative_position + start_position;
    }

    private void Awake()
    {
        text = GetComponent<TMPro.TMP_Text>();
        start_color = text.color;
        rect = GetComponent<RectTransform>();
        if (type == SmartTextType.Fade) text.color = Color.clear;
        start_position = transform.position;
        start_anchoredPosition = rect.anchoredPosition;
        if (follow_transform) start_position = transform.position - follow_transform.position;
    }
}
