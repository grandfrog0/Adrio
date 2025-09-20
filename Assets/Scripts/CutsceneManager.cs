using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class CutsceneManager : MonoBehaviour
{
    public enum CutsceneType{Default, Cinema, DontFreezePlayers, CinemaAndDontFreezePlayers}

    [SerializeField] private PlayableDirector anim;
    [SerializeField] private List<PlayableAsset> timelines;
    [SerializeField] private List<float> stop_time;
    [SerializeField] private CutsceneType type_ = CutsceneType.Default;
    [SerializeField] private Inputs inputs;

    public void Play(int cur)
    {
        anim.Play(timelines[cur]);
        
        switch (type_)
        {
            case CutsceneType.Default:
                inputs.FreezeForTime(stop_time[cur], true);
                break;
        }
    }

    public void SetType(CutsceneType type_)
    {
        this.type_ = type_;
    }
}
