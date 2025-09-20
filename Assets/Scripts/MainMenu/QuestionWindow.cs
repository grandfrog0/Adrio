using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestionWindow : MonoBehaviour
{
    [SerializeField] private RectTransform window, outline;
    [SerializeField] private Vector3 need_size;
    [SerializeField] private AudioSource close_au;
    [SerializeField] private bool has_mouse = false, opened = false;
    private int animation_step = 0;
    [SerializeField] private float elactic_k = 1.1f, speed=8;
    [SerializeField] private RectTransform text_rect;
    [SerializeField] private TMPro.TMP_Text text;
    [SerializeField] private Vector3 text_pos;
    [SerializeField] private Vector2 text_size;
    [SerializeField] private float font_size = 36;
    private LanguageSection section;
    [SerializeField] private string translateNaming;

    public void Load()
    {
        opened = true;
        animation_step = 1;
        window.localScale = need_size * elactic_k;
        outline.localScale = need_size * elactic_k;
        outline.sizeDelta = window.sizeDelta + new Vector2(25 / (need_size.x * elactic_k), 25 / (need_size.y * elactic_k));

        text_rect.localPosition = text_pos;
        
        section = Translator.GetSection("question_texts");
        Debug.Log(section);
        
        if (section.content.ContainsKey(translateNaming)) 
            text.text = section.content[translateNaming].Replace("\\n", "\n");

        text.fontSize = font_size;
    }

    public void Close()
    {
        opened = false;
        window.localScale = Vector3.zero;
        outline.localScale = Vector3.zero;
    }

    public void HasMouse(bool value)
    {
        has_mouse = value;
    }

    void Update()
    {
        if (!opened) return;
        if (!has_mouse && (Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1) || Input.GetMouseButtonDown(2)))
        {
            Close();
            if (close_au) close_au.Play();
        }
        if (Input.GetKeyDown(KeyCode.Escape)) Close();
    }

    void FixedUpdate()
    {
        if (opened)
        {
            if (animation_step == 1)
            {
                window.localScale = Vector3.Lerp(window.localScale, need_size, speed*Time.fixedDeltaTime);
                outline.localScale = Vector3.Lerp(outline.localScale, need_size, speed*Time.fixedDeltaTime);
                outline.sizeDelta = Vector2.Lerp(outline.sizeDelta, window.sizeDelta + new Vector2(25 / need_size.x, 25 / need_size.y), speed*Time.fixedDeltaTime);
                if (Vector3.Distance(window.localScale, need_size) < 0.01f)
                {
                    window.localScale = need_size;
                    outline.localScale = need_size;
                    outline.sizeDelta = window.sizeDelta + new Vector2(25 / need_size.x, 25 / need_size.y);

                    animation_step = 2;
                }
            }

            text_rect.sizeDelta = window.sizeDelta + text_size;
        }
    }

    private void Awake()
    {
        Close();
    }
}
