using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Choicer : MonoBehaviour
{
    public RectTransform scroll_view_variants;
    public Vector3 closed_pos, opened_pos;
    public Vector2 closed_scale, opened_scale;
    public bool variants_view_opened;
    public float speed = 8;
    public TMPro.TMP_Text chosen_text;

    public List<TMPro.TMP_Text> choices_texts;
    public List<string> values;
    public MonoBehaviour obj;
    public string message;

    void FixedUpdate()
    {
        if (variants_view_opened)
        {
            scroll_view_variants.localPosition = Vector3.Lerp(scroll_view_variants.localPosition, opened_pos, speed * Time.fixedDeltaTime);
            scroll_view_variants.sizeDelta = Vector2.Lerp(scroll_view_variants.sizeDelta, opened_scale, speed * Time.fixedDeltaTime);
        }
        else
        {
            scroll_view_variants.localPosition = Vector3.Lerp(scroll_view_variants.localPosition, closed_pos, speed * Time.fixedDeltaTime);
            scroll_view_variants.sizeDelta = Vector2.Lerp(scroll_view_variants.sizeDelta, closed_scale, speed * Time.fixedDeltaTime);
        }
    }

    public void SwitchOpened()
    {
        variants_view_opened = !variants_view_opened;
    }

    public void SetOpened(bool value)
    {
        variants_view_opened = value;
    }

    public void SetMomentalOpened(bool value)
    {
        variants_view_opened = value;
        if (variants_view_opened)
        {
            scroll_view_variants.localPosition = opened_pos;
            scroll_view_variants.sizeDelta = opened_scale;
        }
        else
        {
            scroll_view_variants.localPosition = closed_pos;
            scroll_view_variants.sizeDelta = closed_scale;
        }
    }

    public void ChoiceGet(int index)
    {
        obj.SendMessage(message, values[index]);
        chosen_text.text = choices_texts[index].text;
    }
}
