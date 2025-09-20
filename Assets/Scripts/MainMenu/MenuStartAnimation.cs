using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuStartAnimation : MonoBehaviour
{
    [SerializeField] private int order;
    [SerializeField] private List<RectTransform> objects;
    // [SerializeField] private List<MonoBehaviour> scripts_to_activate;
    [SerializeField] private List<Vector3> objects_need_pos, objects_need_scale;
    [SerializeField] private List<Vector3> objects_start_pos, objects_start_scale;
    [SerializeField] private float speed;
    [SerializeField] private List<float> between_time;


    // [SerializeField] private AudioSource music;

    void Start()
    {
        for(int i = 0; i < objects.Count; i++)
        {
            objects_start_pos.Add(objects[i].anchoredPosition);
            objects_start_scale.Add(objects[i].localScale);
            objects[i].anchoredPosition = objects_need_pos[i];
            objects[i].localScale = objects_need_scale[i];
            // if (scripts_to_activate[i]) scripts_to_activate[i].enabled = false;
        }
    }

    void FixedUpdate()
    {
        if (order < objects.Count)
        {
            for (int i = 0; i <= order; i++)
            {
                objects[i].anchoredPosition = Vector3.Lerp(objects[i].anchoredPosition, objects_start_pos[i], speed * Time.fixedDeltaTime);
                objects[i].localScale = Vector3.Lerp(objects[i].localScale, objects_start_scale[i], speed * Time.fixedDeltaTime);
            }
            if (between_time[order] > 0) between_time[order] -= Time.fixedDeltaTime;
            else
            {
                order++;
                // if (order >= objects.Count - 1) foreach (MonoBehaviour script in scripts_to_activate) if (script) script.enabled = true;
            }
        }

        // if (SceneTransmitter.need_scene != -1 && SceneTransmitter.need_scene != -3)
        // {
        //     music.volume = Mathf.Lerp(music.volume, 0, 0.1f);
        //     music.pitch = Mathf.Lerp(music.pitch, 0, 0.1f);
        // }
    }
}
