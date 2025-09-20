using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class SceneTransmitter
{
    private static int load_type = -1; 
    private static float music_time = -1; 
    public static int need_scene = -3;
    public static int last_load_type = 0;

    public static void SetLoadType(int value, bool save_time)
    {
        load_type = value;
        if (save_time) 
        {
            music_time = GameObject.FindGameObjectWithTag("GameController").GetComponent<MusicManager>().GetTime();
            // Debug.Log("Music time saved " + music_time);
        }
    }

    public static int GetLoadType()
    {
        int value = load_type;
        load_type = -1;
        return value;
    }

    public static float GetMusicTime()
    {
        float value = music_time;
        music_time = -1;
        // Debug.Log("Music time set " + value);
        return value;
    }

    public static int GetActiveSceneIndex()
    {
        return SceneManager.GetActiveScene().buildIndex;
    }
}
