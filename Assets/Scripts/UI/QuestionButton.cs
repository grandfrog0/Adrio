using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuestionButton : MonoBehaviour
{
    [SerializeField] private Image button, outline;
    [SerializeField] private Sprite button_def, button_clicked, outline_def, outline_clicked;
    [SerializeField] private RectTransform button_rect;
    private bool clicked;

    public void Clicked(bool clicked)
    {   
        if (this.clicked == clicked) return;
        this.clicked = clicked;

        if (clicked)
        {
            button.sprite = button_clicked;
            outline.sprite = outline_clicked;
            button_rect.anchoredPosition -= Vector2.up * 5;
        }
        else
        {
            button.sprite = button_def;
            outline.sprite = outline_def;
            button_rect.anchoredPosition += Vector2.up * 5;
        }
    }
}
