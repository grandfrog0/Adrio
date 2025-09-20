using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomHpBar : MonoBehaviour
{
    public Transform mask;
    public BaseEntity entity;
    public float min_hp, max_hp;
    public float min_x, delta_x;
    public bool died, dealth_animated;

    void Start()
    {
        max_hp = entity.GetHp();
        // Debug.Log(entity.GetHp());
    }

    void FixedUpdate()
    {
        if (dealth_animated || max_hp == 0) transform.localScale = Vector3.Lerp(transform.localScale, new Vector3(0, transform.localScale.y), 3*Time.deltaTime);
        if (max_hp == 0) return;
        min_hp = entity.GetHp();
        if (min_hp > 0)
        {
            mask.localPosition = Vector3.Lerp(mask.localPosition, new Vector3(min_x + min_hp / max_hp * delta_x, mask.localPosition.y), 3*Time.deltaTime);
            transform.localScale = Vector3.Lerp(transform.localScale, new Vector3(2.125f, transform.localScale.y), 1*Time.deltaTime);
        }
        else
        {
            if (!died)
            {
                died = true;
                Invoke("animate", 0.5f);
            }
            mask.localPosition = Vector3.Lerp(mask.localPosition, new Vector3(min_x, mask.localPosition.y), 3*Time.deltaTime);
        }
    }

    void animate()
    {
        dealth_animated = true;
    }
}
