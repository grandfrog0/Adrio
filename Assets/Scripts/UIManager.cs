using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] private GameObject hp_bar_heart_prefab;
    public List<BaseEntity> players;
    [SerializeField] private Image hp_bar;
    [SerializeField] private RectTransform rect_hp_bar;
    [SerializeField] private AudioSource hp_add_sound;
    public List<Sprite> hp_bar_images;
    public List<int> hp_bar_values;
    public int cur_player;
    public bool hearts_seted_awake=false;
    [SerializeField] private int heart_spawned_cur;
    [SerializeField] private SceneLoaderManager scene_loader_manager;
    [SerializeField] private List<GameObject> hp_bar_hearts = new List<GameObject>();
    [SerializeField] private List<bool> hp_bar_hearts_actived = new List<bool>();
    private List<Animator> hp_bar_hearts_animator = new List<Animator>();
    [SerializeField] private HpBarAnimation hp_bar_animation;
    [SerializeField] private Sprite queue_hp_bar_image;
    [SerializeField] private Inputs inputs;

    private Transform power_mask, power_bar;
    private float mask_x_multiplier = 0.3f;
    private Vector3 power_mask_start; //(y)-0.15f is full, -1.1f is null, definate is +0.95f or ~1
    [SerializeField] private bool power_bar_activated = false; //-12f -> -9.75f (y=3.2f, z=10f)

    private void Awake()
    {
        Invoke("SpawnHeart", 2f);

        power_mask = GameObject.FindGameObjectWithTag("PowerMask").transform;
        power_mask_start = power_mask.localPosition;
        power_bar = GameObject.FindGameObjectWithTag("PowerBar").transform;
    }

    public void ResetPlayerSpawnedHearts(int value)
    {
        cur_player = value;
        if (players[cur_player].GetHp() > heart_spawned_cur)
        {
            for (int i = 0; i < players[cur_player].GetHp(); i++)
            {
                if (i >= heart_spawned_cur)
                {
                    if (i < hp_bar_hearts.Count)
                    {
                        hp_bar_hearts[i].SetActive(true); 
                        hp_bar_hearts_actived[i] = true;
                        heart_spawned_cur++;
                    }
                    else
                    {
                        SpawnHeart0();
                    }
                }
            }
        }
        if (players[cur_player].GetHp() < heart_spawned_cur)
        {
            for (int i = 0; i < heart_spawned_cur; i++)
            {
                if (i >= players[cur_player].GetHp())
                {
                    hp_bar_hearts[i].SetActive(false);
                    hp_bar_hearts_actived[i] = false;
                }
            }
        }
        heart_spawned_cur = players[cur_player].GetHp();
    }

    public void OnHpBarAnimationEnd()
    {
        hp_bar.sprite = queue_hp_bar_image;
        hp_bar.SetNativeSize();
        queue_hp_bar_image = null;
        // ResetPlayerSpawnedHearts(cur_player);
    }

    public void PlayHpBarAnim()
    {
        if (queue_hp_bar_image != hp_bar_images[cur_player]) 
        {
            queue_hp_bar_image = hp_bar_images[cur_player];

            if (hp_bar_values[cur_player] >= hp_bar_values[hp_bar_images.IndexOf(hp_bar.sprite)]) hp_bar_animation.PlayExpand();
            else hp_bar_animation.PlayNarrow();
        }
    }

    void FixedUpdate()
    {
        if (scene_loader_manager.SceneLoaded()) rect_hp_bar.anchoredPosition = Vector2.Lerp(rect_hp_bar.anchoredPosition, new Vector2(10, -10), 0.3f);
        else rect_hp_bar.anchoredPosition = Vector2.Lerp(rect_hp_bar.anchoredPosition, new Vector2(-250, -10), 0.3f);

        foreach (BaseEntity player in players) if (!player.IsLive())
        {
            SceneTransmitter.need_scene = -2;
            SceneTransmitter.SetLoadType(SceneTransmitter.last_load_type, true);
        }
        if (heart_spawned_cur > players[cur_player].GetHp()) 
        {
            heart_spawned_cur--;
            hp_bar_hearts_animator[GetLastHpBarHeart()].Play("pop");
            hp_bar_hearts_actived[GetLastHpBarHeart()] = false;
            inputs.StopSetPlayerForTime(0.23f * 100 / 60);
        }
        if (hearts_seted_awake && heart_spawned_cur < players[cur_player].GetHp())
        {
            heart_spawned_cur++;
            // if (heart_spawned_cur < hp_bar_hearts.Count) hp_bar_hearts[hp_bar_hearts.Count-1].SetActive(true);
            // else
            // {
                hp_bar_hearts.Add(Instantiate(hp_bar_heart_prefab, rect_hp_bar));
                hp_bar_hearts[heart_spawned_cur].transform.localPosition = new Vector3(43.75f + 75*heart_spawned_cur, -43.75f);
            // }
        }

        if (hp_bar.sprite != hp_bar_images[cur_player]) 
        {
            PlayHpBarAnim();
        }

        power_mask.localPosition = Vector3.Lerp(power_mask.localPosition, new Vector3(power_mask_start.x + mask_x_multiplier, -1.1f + players[cur_player].movement.GetSuperChargedProcent(), power_mask_start.z), 0.05f);
        if (Vector3.Distance(power_mask.localPosition, new Vector3(power_mask_start.x + mask_x_multiplier, -1.1f + players[cur_player].movement.GetSuperChargedProcent(), power_mask_start.z)) < 0.02f) mask_x_multiplier *= -1;
        if (power_bar_activated && scene_loader_manager.SceneLoaded()) power_bar.localPosition = Vector3.Lerp(power_bar.localPosition, new Vector3(-9.75f, 3.2f, 10f), 0.3f);
        else power_bar.localPosition = Vector3.Lerp(power_bar.localPosition, new Vector3(-12f, 3.2f, 10f), 0.3f);
    }

    private int GetLastHpBarHeart()
    {
        for (int i = 0; i < hp_bar_hearts.Count; i++)
        {
            if (!hp_bar_hearts[i].activeSelf || !hp_bar_hearts_actived[i]) return i-1;
        }
        return hp_bar_hearts.Count-1;
    }

    private void SpawnHeart()
    {
        SpawnHeart0();
        hp_bar_hearts_animator[hp_bar_hearts_animator.Count-1].Play("spawn");
        hp_add_sound.Play();
        
        if (heart_spawned_cur < players[cur_player].GetHp()) Invoke("SpawnHeart", 0.3f);
        else hearts_seted_awake = true;
    }

    private void SpawnHeart0()
    {
        hp_bar_hearts.Add(Instantiate(hp_bar_heart_prefab, hp_bar.gameObject.transform));
        hp_bar_hearts[heart_spawned_cur].transform.localPosition = new Vector3(43.75f + 75*heart_spawned_cur, -43.75f);
        hp_bar_hearts_animator.Add(hp_bar_hearts[heart_spawned_cur].GetComponent<Animator>());
        // for(int i = 0; i < hp_bar_hearts.Count; i++) hp_bar_hearts_actived.Add(true);
        hp_bar_hearts_actived.Add(true);
        heart_spawned_cur++;
    }

    public void ActivatePower(bool value)
    {
        power_bar_activated = value;
    }
}
