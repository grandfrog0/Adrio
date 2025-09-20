using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuManager : MonoBehaviour, IInitializable
{
    [SerializeField] BetweenWindowsAnimation anim;

    [SerializeField] List<Vector3> entity_positions;
    [SerializeField] List<GameObject> entities;
    [SerializeField] List<int> entity_completed_level;
    [SerializeField] int lastPos = 0;

    [SerializeField] MusicManager music_manager;
    [SerializeField] List<string> music_names;
    [SerializeField] List<string> music_texts;

    [SerializeField] SceneLoaderManager scene_loader_manager;
    [SerializeField] TranslateManager translate_manager;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) anim.GoWindow(0);
    }

    public void SetSoundMuted(bool value)
        => DataManager.Game.isSoundMuted = value;
    public void SetMusicMuted(bool value)
        => DataManager.Game.isMusicMuted = value;

    public InitializeOrder Order => InitializeOrder.MenuManager;
    public void Initialize()
    {
        CheckMenuMusic();

        if (DataManager.Game.openedLevels.Count == 0)
            DataManager.Game.openedLevels.Add(1);

        foreach(GameObject entity in entities) 
            entity.SetActive(false);

        int attempts = 1000;
        while(attempts-- > 0)
        {
            if (DataManager.Game.completedLevels.Count == 0 || lastPos > entity_positions.Count - 1) 
                break;

            if (MyMath.Chance(25)) break;

            int rnd = Random.Range(0, entities.Count);
            if (!DataManager.Game.completedLevels.Contains(entity_completed_level[rnd])) 
                continue;

            if (entities[rnd].activeSelf) continue;
            entities[rnd].SetActive(true);

            entities[rnd].transform.position = entity_positions[lastPos++];
            if (MyMath.Chance(50)) entities[rnd].transform.localScale = new Vector3(-1, 1, 1);

        }
        if (attempts <= 0)
            Debug.Log("Error occured: infinity cycle.");
    }

    public void CheckMenuMusic()
    {
        int value = music_names.IndexOf(DataManager.Game.menuMusic);
        if (value != -1) music_manager.SetClip(value);
    }

    public void SetMenuMusic(string naming)
    {
        DataManager.Game.menuMusic = naming;
        CheckMenuMusic();
    }

    public string GetMusicTextByName(string naming)
    {
        if (music_names.IndexOf(naming) == -1) return music_texts[0];
        return music_texts[music_names.IndexOf(naming)];
    }

    public void PlayLastOpenedLevel() //it gonna be delete later, when i'l make levels window
    {
        int last_level = -1;
        List<int> levels = MyMath.Sorted(DataManager.Game.openedLevels);
        while (last_level == -1 || last_level > scene_loader_manager.GetScenesCount() - 1)
        {
            last_level = MyMath.Max(levels);
            levels = levels.GetRange(0, levels.Count - 1);
        }
        Debug.Log("Go to scene " + last_level);
        scene_loader_manager.GoScene(last_level);
    }

    public void SetLanguage(string value)
    {
        DataManager.Game.language = value;
        translate_manager.Check();
    }
}
