using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BetweenWindowsAnimation : MonoBehaviour
{
    public Image fade;
    public float fade_speed = 5;
    public int need_window;
    public List<MonoBehaviour> windows;
    public List<int> active_windows;
    public TranslateManager translate_manager;

    public AnimationOnClick snowman;

    void Start()
    {
        foreach (MonoBehaviour obj in windows) if (obj) obj.SendMessage("Close");
    }

    void FixedUpdate()
    {
        if (need_window != -1)
        {
            fade.color = Color.Lerp(fade.color, Color.black, fade_speed*Time.fixedDeltaTime);
            if (fade.color.a >= 0.95f)
            {
                if (need_window != 0)
                {
                    windows[need_window].SendMessage("Load");
                    translate_manager.Check();
                    if (active_windows.IndexOf(need_window) == -1) active_windows.Add(need_window);
                }
                else foreach(int index in active_windows) windows[index].SendMessage("Close");

                snowman.active = need_window == 0;

                fade.raycastTarget = false;
                need_window = -1;

            }
        }
        else
        {   
            if (fade.color.a > 0) fade.color = Color.Lerp(fade.color, new Color(0, 0, 0, 0), fade_speed*Time.fixedDeltaTime);
        }
    }

    public void GoWindow(int value)
    {
        need_window = value;
        fade.raycastTarget = true;
    }
}
