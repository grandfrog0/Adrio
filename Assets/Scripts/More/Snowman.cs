using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Snowman : AnimationOnClick
{
    [SerializeField] private SpriteRenderer snowman;
    [SerializeField] private Sprite snowman_chokipie, snowman_dark, achievement_icon;
    private bool is_dark = false;
    [SerializeField] private GameObject item;
    
    private void Awake()
    {
        is_dark = false;
        if (MyMath.Chance(5))
        {
            snowman.sprite = snowman_chokipie;
            if (MyMath.Chance(5))
            {
                snowman.sprite = snowman_dark;
                is_dark = true;
            }
        }
    }

    public void Clicked()
    {
        if (is_dark && !EventMessageManager.AchievementCompleted("dark_snowman_clicked"))
        {
            EventMessageManager.Main().GetAchievement("dark_snowman_clicked", achievement_icon);
            EventMessageManager.Main().GetItem(item);
        }
    }
}
