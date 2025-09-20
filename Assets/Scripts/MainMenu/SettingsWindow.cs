using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingsWindow : MonoBehaviour
{
    [SerializeField] private GameObject window_object;
    [SerializeField] private List<RectTransform> animation_images;
    [SerializeField] private List<Vector3> start_pos = new List<Vector3>();
    [SerializeField] private List<float> add_x = new List<float>();
    [SerializeField] private float range = 8, floor = -20, speed=5;
    [SerializeField] private Toggler toggle_music, toggle_sounds;
    [SerializeField] private Choicer choicer_menu_music, choicer_language;
    [SerializeField] private TMPro.TMP_Text text_menu_music, text_language;
    [SerializeField] private MenuManager menu_manager;

    public void Load()
    {
        window_object.SetActive(true);
        add_x.Clear();
        for(int i = 0; i < animation_images.Count; i++)
        {
            add_x.Add(Random.Range(-range, range));
            animation_images[i].localPosition = new Vector3(start_pos[i].x + add_x[i], floor);
        }
        toggle_music.SetValue(!DataManager.Game.isMusicMuted);
        toggle_sounds.SetValue(!DataManager.Game.isSoundMuted);
        choicer_menu_music.SetMomentalOpened(false);
        choicer_language.SetMomentalOpened(false);
        text_menu_music.SetText(menu_manager.GetMusicTextByName(DataManager.Game.menuMusic));
        if (DataManager.Game.language != "") text_language.SetText(choicer_language.choices_texts[choicer_language.values.IndexOf(DataManager.Game.language)].text);
        else text_language.SetText(choicer_language.choices_texts[0].text);
    }

    public void Close()
    {
        window_object.SetActive(false);
    }

    void FixedUpdate()
    {
        if (add_x.Count == 0) return;
        for(int i = 0; i < animation_images.Count; i++)
        {
            animation_images[i].localPosition = Vector3.Lerp(animation_images[i].localPosition, start_pos[i], speed * Time.fixedDeltaTime);
        }
    }

    void Awake()
    {
        for(int i = 0; i < animation_images.Count; i++) start_pos.Add(animation_images[i].localPosition);
    }
}
