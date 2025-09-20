using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PressureButton : MonoBehaviour
{
    enum DataType {Nothing, ValueFloat, Text, ValueInt, Boolean, Transform};

    [SerializeField] private List<DataType> data_type;
    [SerializeField] private List<MonoBehaviour> obj;
    [SerializeField] private List<string> message;

    [SerializeField] List<float> value_float;
    [SerializeField] List<string> text;
    [SerializeField] List<int> value_int;
    [SerializeField] List<bool> boolean;
    [SerializeField] List<Transform> transform_;
    [SerializeField] List<GameObject> obj_;

    [SerializeField] private Transform button_transform;
    [SerializeField] private float button_pressed_y;
    [SerializeField] private bool pressed;
    // [SerializeField] private bool press_to_forever = true;
    [SerializeField] private bool animation_button_pressed_is_playing;
    [SerializeField] private float anim_speed = 2;

    private void FixedUpdate()
    {
        if (pressed) return;

        if (animation_button_pressed_is_playing && button_transform.localPosition.y > button_pressed_y)
        {
            button_transform.localPosition -= transform.up * Time.fixedDeltaTime * anim_speed;
        }

        // if (button_transform.localPosition.y <= button_pressed_y)
        // {
        //     pressed = true;
        //     Invoke("Pressed", 0.5f);
        // }
    }

    private void Pressed()
    {
        if (pressed) return;
        pressed = true;
        for(int i = 0; i < obj.Count; i++)
        {
            switch (data_type[i])
            {
                case DataType.Nothing:
                    obj[i].SendMessage(message[i]);
                    break;

                case DataType.ValueFloat:
                    obj[i].SendMessage(message[i], value_float[i]);
                    break;
                        
                case DataType.Text:
                    obj[i].SendMessage(message[i], text[i]);
                    break;
                        
                case DataType.ValueInt:
                    obj[i].SendMessage(message[i], value_int[i]);
                    break;
                        
                case DataType.Boolean:
                    obj[i].SendMessage(message[i], boolean[i]);
                    break;
                        
                case DataType.Transform:
                    obj[i].SendMessage(message[i], transform_[i]);
                    break;
            }
        }
    }

    void OnTriggerEnter2D(Collider2D coll)
    {
        // if (coll.gameObject.CompareTag("Player"))
        if (coll.gameObject.GetComponent<BaseEntity>() != null)
        {
            Invoke("Pressed", 0.5f);
            animation_button_pressed_is_playing = true;
        }
    }
}
