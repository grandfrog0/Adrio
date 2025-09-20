using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonAnimation : MonoBehaviour
{
    [SerializeField] private RectTransform button, image;
    [SerializeField] private Vector3 start_pos_button, start_pos_image;
    [SerializeField] private Vector3 add_vector;
    [SerializeField] private float vector_button_multiplier = 12.5f, vector_image_multiplier = 12.5f;
    [SerializeField] private float speed = 10;
    [SerializeField] private bool mouse_inside, clicked;

    public void OnMouseEnter()
    {
        mouse_inside = true;
    }

    public void OnMouseExit()
    {
        mouse_inside = false;
    }

    public void OnMouseDown()
    {
        clicked = true;
    }

    public void OnMouseUp()
    {
        clicked = false;
    }

    private void FixedUpdate()
    {
        add_vector = mouse_inside && !clicked ? Vector3.up : Vector3.zero;
        button.localPosition = Vector3.Lerp(button.localPosition, start_pos_button + add_vector * vector_button_multiplier, speed * Time.fixedDeltaTime);
        image.localPosition = Vector3.Lerp(image.localPosition, start_pos_image + add_vector * vector_image_multiplier, speed * Time.fixedDeltaTime);
    }

    private void Awake()
    {
        start_pos_button = button.localPosition;
        start_pos_image = image.localPosition;
    }
}
