using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoaderManager : MonoBehaviour
{
    [Header("Data load")]
    [SerializeField] private List<Transform> objects;
    [SerializeField] private List<Vector3> positions;
    [SerializeField] private List<int> borders;
    [SerializeField] private MusicManager music_manager;
    [Header("Fade animation")]
    [SerializeField] private Inputs inputs;
    [SerializeField] private bool opened = true;
    [SerializeField] private bool anim_ended;
    [SerializeField] private AudioSource build_sound;
    [SerializeField] private GameObject bricks_parent;
    private Animator load_anim;
    private LoadSceneBlockAnimation load_blocks;

    void Awake()
    {
        load_anim = bricks_parent.GetComponent<Animator>();
        load_blocks = bricks_parent.GetComponent<LoadSceneBlockAnimation>();
        bricks_parent.SetActive(true);
        opened = true;
        if (SceneTransmitter.need_scene == -3) load_anim.Play("down");

        int load_type = SceneTransmitter.GetLoadType();
        if (SceneManager.GetActiveScene().buildIndex == 0) load_type = -1;
        SceneTransmitter.need_scene = -1;
        
        if (load_type >= 0) SceneTransmitter.last_load_type = load_type;
        float music_time = SceneTransmitter.GetMusicTime();
        if (music_time != -1 && music_manager) music_manager.LoadMusic(load_type, music_time);
        if (load_type != -1) for (int i = load_type != 0 ? borders[load_type - 1] : 0; i < borders[load_type]; i++)
        {
            objects[i].position = positions[i];
        }
    }

    public bool SceneLoaded()
    {
        return opened && anim_ended;
    }

    void FixedUpdate()
    {
        if (bricks_parent.activeSelf) load_anim.SetBool("up", !opened);
        anim_ended = load_anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1 && !load_anim.IsInTransition(0);
        if (SceneTransmitter.need_scene != -1 && SceneTransmitter.need_scene != -3 && opened)
        {
            opened = false;
            if (inputs) inputs.SetPlayer(-1);
            load_blocks.Check();
            anim_ended = false;
            load_anim.Play("go_up");
        }
        if (opened)
        {
            if (!anim_ended && !build_sound.isPlaying) build_sound.Play();
        }
        else
        {
            if (anim_ended)
            {
                if (SceneTransmitter.need_scene != -1) 
                {
                    if (SceneTransmitter.need_scene == -2) SceneManager.LoadScene(SceneManager.GetActiveScene().name);
                    else if (SceneTransmitter.need_scene >= GetScenesCount()) SceneManager.LoadScene(0);
                    else SceneManager.LoadScene(SceneTransmitter.need_scene);
                }
            }
            else
            {
                if (!build_sound.isPlaying) build_sound.Play();
                if (music_manager) music_manager.FadeVolume();
            }
        }
    }

    public int GetScenesCount()
    {
        return SceneManager.sceneCountInBuildSettings;
    }

    public void GoScene(int to_scene)
    {
        SceneTransmitter.SetLoadType(0, false);
        SceneTransmitter.need_scene = to_scene;
    }
}
