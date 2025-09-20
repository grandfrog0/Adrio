using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour, IInitializable
{
    [SerializeField] private List<AudioClip> clips;
    [SerializeField] private AudioSource au;
    [SerializeField] private List<int> load_indexes;
    [SerializeField] private float start_volume;
    [SerializeField] private float start_pitch;
    [SerializeField] private List<AudioClip> music_queue = new List<AudioClip>();
    public bool fade_volume;
    public bool fade_volume_on_awake = true;

    private bool au_muted;
    private List<bool> sounds_muted = new List<bool>();
    private List<AudioSource> sounds = new List<AudioSource>();

    public InitializeOrder Order => InitializeOrder.MusicManager;
    public void Initialize()
    {
        au_muted = au.mute;
        AudioSource[] audios = FindObjectsOfType<AudioSource>();
        foreach(AudioSource audio in audios) if (audio != au) sounds.Add(audio);
        foreach(AudioSource audio in sounds) sounds_muted.Add(audio.mute);
        CheckAudioMuted();

        start_volume = au.volume;
        start_pitch = au.pitch;
        if (fade_volume_on_awake)
        {
            au.volume = 0;
            au.pitch = 0;
        }

        foreach(AudioClip clip in clips)
        {
            clip.LoadAudioData();
        }
    }

    void FixedUpdate()
    {
        if (!fade_volume) {au.volume = Mathf.Lerp(au.volume, start_volume, 0.1f); au.pitch = Mathf.Lerp(au.pitch, start_pitch, 0.1f);}
        else {au.volume = Mathf.Lerp(au.volume, 0, 0.1f); au.pitch = Mathf.Lerp(au.pitch, 0, 0.1f);}
        
        if (au.clip != null)
        {
            if (!au.isPlaying)
            {
                if (music_queue.Count == 0) au.Play();
                else SetNextClip();
            }
        }
        else if (music_queue.Count == 0) SetNextClip();
    }

    private void SetNextClip()
    {
        if (music_queue.Count == 0) return;
        au.clip = music_queue[0];
        music_queue.RemoveAt(0);
        au.Play();
    }

    public void AddClipToQueueByIndex(int value)
    {
        music_queue.Add(clips[value]);
    }

    public void SetClip(int value)
    {
        au.clip = clips[value];
        au.Play();
    }

    public void LoadMusic(int value, float time)
    {
        if (load_indexes[value] == -1)
        {
            fade_volume = true;
            return;
        }

        SetClip(load_indexes[value]);
        au.time = time;
        // Debug.Log("Music is seted as " + time + "; " + au.time);
    }

    public float GetTime()
    {
        return au.time;
    }

    public void FadeVolume()
    {
        fade_volume = true;
    }

    public void CheckAudioMuted()
    {
        au.mute = au_muted || DataManager.Game.isMusicMuted;
        bool mute_sounds = DataManager.Game.isSoundMuted;
        for(int i = 0; i < sounds.Count; i++) if (sounds[i]) sounds[i].mute = sounds_muted[i] || mute_sounds;
    }
}
