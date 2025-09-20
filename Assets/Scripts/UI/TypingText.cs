using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TypingText : MonoBehaviour
{
    public string text;
    public TMPro.TMP_Text text_script;
    public float speed;
    private float index;
    public bool set_text_on_awake;

    private void FixedUpdate()
    {
        if ((int) index < text.Length) index += Time.fixedDeltaTime * speed;
        if ((int) index > text.Length) index = text.Length - 1;
        text_script.text = text.Substring(0, (int) index);
    }

    public void SetText(string value)
    {
        text = value;
        index = 0;
    }

    private void Awake()
    {
        if (set_text_on_awake)
        {
            text_script.text = text;
            index = text.Length - 1;
        }
    }
}
