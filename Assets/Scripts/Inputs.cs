using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inputs : MonoBehaviour
{
    [SerializeField] List<Movement> player_movements;
    [SerializeField] List<BaseEntity> player_entities;
    [SerializeField] List<BaseEntityAnimator> player_animators;
    [SerializeField] int cur_player;
    [SerializeField] List<KeyCode> player_codes;
    [SerializeField] List<Sprite> hp_bar_images;
    [SerializeField] List<int> hp_bar_values;
    [SerializeField] bool can_set_player = true;
    [SerializeField] Transform player_tracker;
    [SerializeField] AudioSource player_switched_sound;
    [SerializeField] List<int> players_order; // 0 is the most-time disactive, Count-1 is active;
    [SerializeField] PersonalizationManager personalizationManager;

    private CameraWatch cam;
    private UIManager ui;

    private bool canControlPlayers = true;
    private int lastPlayer = -1;

    private void Update()
    {
        if (cur_player >= 0 && cur_player < player_movements.Count) 
        {
            if (canControlPlayers)
            {
                player_movements[cur_player].move_input_axis = GetInputValue(new KeyCode[]{KeyCode.A, KeyCode.D, KeyCode.LeftArrow, KeyCode.RightArrow}, new float[]{-1, 1,-1, 1});
                player_movements[cur_player].move_input_axis_vertical = GetInputValue(new KeyCode[]{KeyCode.S, KeyCode.W, KeyCode.DownArrow, KeyCode.UpArrow}, new float[]{-1, 1,-1, 1});
                if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow)) player_movements[cur_player].Jump();
                if (player_movements[cur_player].super_keycodes.Count > 0)
                {
                    if (player_movements[cur_player].keycode_down)
                    {
                        foreach (KeyCode code in player_movements[cur_player].super_keycodes)
                        {
                            if (Input.GetKeyDown(code)) player_movements[cur_player].TryToSuper();
                        }
                    }
                    if (!player_movements[cur_player].keycode_down)
                    {
                        foreach (KeyCode code in player_movements[cur_player].super_keycodes)
                        {
                            if (Input.GetKey(code)) player_movements[cur_player].TryToSuper();
                        }
                    }
                }
            
                if (player_tracker) player_tracker.transform.position = player_movements[cur_player].transform.position;
                foreach(Item item in player_entities[cur_player].items)
                {
                    if (item.use_keycode.Count == 0) continue;
                    foreach(KeyCode code in item.use_keycode)
                    {
                        if (Input.GetKeyDown(code)) item.Use();
                    }
                }
            }
            else
            {
                player_movements[cur_player].move_input_axis = 0;
                player_movements[cur_player].move_input_axis_vertical = 0;
            }

            if (Input.GetKeyDown(KeyCode.F1)) // Reload scene
            {
                SceneTransmitter.need_scene = -2;
                SceneTransmitter.SetLoadType(SceneTransmitter.last_load_type, true);
            }
            if (Input.GetKeyDown(KeyCode.F2)) // Go menu
            {
                SceneTransmitter.need_scene = 0;
                SceneTransmitter.SetLoadType(0, false);
            }
        }

        if (cur_player != -1 && ui && ui.hearts_seted_awake) for(int i = 0; i < player_codes.Count; i++)
        {
            if (Input.GetKeyDown(player_codes[i]) && i != cur_player && i < player_movements.Count) SetPlayer(i);
        }
    }

    public void FreezeForTime(float value, bool save_player)
    {
        StopSetPlayerForTime(value);
        StopManagePlayersForTime(value);
        if (player_animators[cur_player]) player_animators[cur_player].SetEntityActive(false);
        if (save_player) lastPlayer = cur_player;
        Invoke("return_last_player", value);
    }

    public void StopManagePlayersForTime(float value)
    {
        canControlPlayers = false;
        Invoke("stop_manage_players_for_time_0", value);
    }

    public void StopSetPlayerForTime(float value)
    {
        can_set_player = false;
        Invoke("stop_set_player_for_time_0", value);
    }

    private void stop_set_player_for_time_0()
    {
        can_set_player = true;
    }

    private void stop_manage_players_for_time_0()
    {
        canControlPlayers = true;
    }

    private void return_last_player()
    {
        cur_player = lastPlayer;
        SetPlayer(lastPlayer);
    }
    
    public void SetLastPlayer(int value)
    {
        lastPlayer = value;
        SetPlayer(lastPlayer);
        Debug.Log("Last player: " + value);
    }

    public void SetPlayer(int value)
    {
        if (!can_set_player) return;
        if (cur_player >= 0 && cur_player < player_movements.Count && !player_movements[cur_player].can_move) return;
        if (value >= 0 && value < player_movements.Count && !player_movements[value].gameObject.activeSelf) return;

        if (!cam) cam = Camera.main.gameObject.GetComponent<CameraWatch>();

        if (value != -1)
        {
            players_order.Remove(value);
            players_order.Add(value);
        }

        if (cur_player != -1)
        {
            if (cam.HasTarget(player_tracker ? player_tracker : player_movements[cur_player].transform)) cam.RemoveTarget(player_tracker ? player_tracker : player_movements[cur_player].transform);
            if (value != -1) player_movements[cur_player].move_input_axis = 0;
            if (player_animators[cur_player]) player_animators[cur_player].SetEntityActive(false);
        }

        cur_player = value;
        if (value != -1)
        {
            cam.AddTarget(player_tracker ? player_tracker : player_movements[cur_player].transform);
            if (Vector3.Distance(cam.transform.position, player_movements[cur_player].transform.position) > cam.min_distance) cam.ToEnd();
            cam.SetRb(player_movements[cur_player].rb);
            if (!ui) ui = GameObject.FindGameObjectWithTag("GameController").GetComponent<UIManager>();
            ui.ResetPlayerSpawnedHearts(cur_player);
            ui.ActivatePower(player_movements[cur_player].super_type != Movement.SuperType.Nothing);
            if (player_animators[cur_player]) player_animators[cur_player].SetEntityActive(true);
            
            for(int i = 0; i < players_order.Count; i++)
            {
                if (player_animators[i]) player_animators[i].SetLayer(players_order.IndexOf(i) - players_order.Count + 1);
            }
        }

        if (player_switched_sound) player_switched_sound.Play();
    }

    public Transform GetCurPlayer()
    {
        if (cur_player >= 0 && cur_player < player_movements.Count) return player_entities[cur_player].transform;
        return null;
    }

    public void SwitchPlayerActive(int value)
    {
        if (value >= 0 && value < player_movements.Count) player_movements[value].gameObject.SetActive(!player_movements[value].gameObject.activeSelf);
    }

    public void ReplacePlayer(Movement movement, int index, Sprite hp_bar_image, int value)
    {
        if (index < 0 || index > player_movements.Count - 1) return;

        if (!cam) cam = Camera.main.gameObject.GetComponent<CameraWatch>();
        if (cam && cam.HasTarget(player_tracker ? player_tracker : player_movements[index].transform))
        {
            cam.RemoveTarget(player_tracker ? player_tracker : player_movements[index].transform);
            cam.AddTarget(player_tracker ? player_tracker : movement.transform);
        }

        player_movements[index] = movement;
        hp_bar_images[index] = hp_bar_image;
        hp_bar_values[index] = value;

        player_entities[index] = movement.gameObject.GetComponent<BaseEntity>();
        if (movement.gameObject.GetComponent<BaseEntityAnimator>()) 
        {
            player_animators[index] = movement.gameObject.GetComponent<BaseEntityAnimator>();
            if (index != cur_player)
            {
                player_animators[index].SetEntityActive(false);
            }
            else player_animators[index] = null;
        }
        if (!ui) ui = GameObject.FindGameObjectWithTag("GameController").GetComponent<UIManager>();
        if (ui && ui.players.Count == 0) ui.players = player_entities;
        if (ui && ui.hp_bar_images.Count == 0) ui.hp_bar_images = hp_bar_images;
        if (ui && ui.hp_bar_values.Count == 0) ui.hp_bar_values = hp_bar_values;

        if (index == cur_player) SetPlayer(index);
    }

    private void Awake()
    {
        if (player_tracker) player_tracker.parent = null;

        if (player_entities.Count == 0)
        {
            foreach (Movement move in player_movements)
            {
                player_entities.Add(move.gameObject.GetComponent<BaseEntity>());
            }
        }
        players_order.Clear();
        for (int i = player_entities.Count - 1; i >= 0; i--)
        {
            players_order.Add(i);
        }
        players_order.Remove(cur_player);
        players_order.Add(cur_player);
        if (player_animators.Count == 0)
        {
            foreach (Movement move in player_movements)
            {
                if (move.gameObject.GetComponent<BaseEntityAnimator>()) 
                {
                    player_animators.Add(move.gameObject.GetComponent<BaseEntityAnimator>());
                    if (player_animators.Count - 1 != cur_player)
                    {
                        player_animators[player_animators.Count - 1].SetEntityActive(false);
                    }
                }
                else player_animators.Add(null);
            }
        }

        if (!cam) cam = Camera.main.gameObject.GetComponent<CameraWatch>();
        if (!ui) ui = GameObject.FindGameObjectWithTag("GameController").GetComponent<UIManager>();
        if (ui && ui.players.Count == 0) ui.players = player_entities;
        if (ui && ui.hp_bar_images.Count == 0) ui.hp_bar_images = hp_bar_images;
        if (ui && ui.hp_bar_values.Count == 0) ui.hp_bar_values = hp_bar_values;
        if (ui) ui.cur_player = cur_player;

        if (cam)
        {
            cam.AddTarget(player_tracker ? player_tracker : player_movements[cur_player].transform);
            cam.SetRb(player_movements[cur_player].rb);
        }
    }

    private void Start()
    {
        foreach (BaseEntity player in player_entities)
        {
            personalizationManager.CheckType(MyMath.ToPlayerType(player.GetEntityType()));
        }

        for(int i = 0; i < players_order.Count; i++)
        {
            // Debug.Log(player_entities[i].items.Count);
            player_animators[i].SetLayer(players_order.IndexOf(i) - players_order.Count + 1, true);
        }
    }

    private float GetInputValue(KeyCode[] key_codes, float[] multipliers)
    {
        float value = 0;
        for (int i = 0; i < key_codes.Length; i++)
        {
            if (Input.GetKey(key_codes[i]))
            {
                if (i < multipliers.Length && multipliers[i] != 0) value += multipliers[i];
                else value += 1;
            }
            
        }
        if (value > 0) return 1;
        if (value < 0) return -1;
        return 0;
    }
}
