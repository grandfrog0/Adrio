using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Scene1_7_Manager : MonoBehaviour
{
    public List<Transform> players;
    public List<Vector3> room_positions;
    public CameraWatch cam;
    public float border_left;
    public SpriteRenderer fade;
    public GameObject darkness;
    public UnityEngine.Tilemaps.Tilemap darkness_tilemap;
    public int darkness_step;
    public MusicManager music;
    public float opacity = 0.9f;
    public Boss_Knight boss;
    public Firebird firebird;
    public bool boss_alive = true;
    public Transform chamber;
    public GameObject boss_hp_bar;

    public Transform slimed_chamber_transform;
    public BoxCollider2D slimed_chamber_coll;
    public Rigidbody2D slimed_chamber_rb;
    public AudioSource slimed_chamber_explose;
    public ParticleSystem slimed_chamber_explose_particles, slime_particles;
    public GameObject compressed_chokipie;
    public GameObject chokipie;

    public Sprite icon;

    public void GoToRoom()
    {
        darkness_step = 1;
        SceneTransmitter.last_load_type = 0;
    }

    private void FixedUpdate()
    {
        if (darkness_step == 1)
        {
            fade.color = new Color(0, 0, 0, fade.color.a + Time.fixedDeltaTime);
            if (fade.color.a >= 1)
            {
                Step2();
            }
        }
        if (darkness_step == 3)
        {
            if (darkness_tilemap.color.a <= opacity) darkness_tilemap.color = new Color(0, 0, 0, darkness_tilemap.color.a + 2 * Time.fixedDeltaTime);
            fade.color = new Color(0, 0, 0, fade.color.a - Time.fixedDeltaTime);
            if (fade.color.a <= 0)
            {
                darkness_step = 4;
                music.fade_volume = false;
                music.SetClip(0);

                boss.StartGame();
                Invoke("Firebird_Fire", Random.Range(5, 10));
            }
        }

        if (!boss.IsLive())
        {
            if (boss_alive)
            {
                Invoke("EndGame", 2);
            }
            else
            {
                chamber.position = Vector3.Lerp(chamber.position, new Vector3(chamber.position.x, 18), 3*Time.deltaTime);
            }
        }
    }

    void EndGame()
    {
        if (!boss_alive) return;
        boss_alive = false;
        music.FadeVolume();
        Invoke("SlimedChamberDetach", 2);
    }

    void SlimedChamberDetach()
    {
        slimed_chamber_transform.parent = null;
        slimed_chamber_coll.enabled = true;
        slimed_chamber_rb.bodyType = RigidbodyType2D.Dynamic;
        Invoke("SlimedChamberExplose", 1);
    }

    void SlimedChamberExplose()
    {
        compressed_chokipie.SetActive(true);
        compressed_chokipie.transform.position = slimed_chamber_transform.position;
        slimed_chamber_transform.gameObject.SetActive(false);
        slimed_chamber_explose_particles.Play();
        Invoke("SpawnChokipi", 2.5f);
    }

    void SpawnChokipi()
    {
        chokipie.SetActive(true);
        chokipie.transform.position = compressed_chokipie.transform.position + Vector3.up;
        compressed_chokipie.SetActive(false);
        slime_particles.Play();
        darkness.SetActive(false);
        darkness.SetActive(true);

        EventMessageManager.Main().GetAchievement("boss_knight_completed", icon);
    }

    public void Step2()
    {
        fade.color = new Color(0, 0, 0, 1);
        darkness_step = 2;
        darkness.SetActive(true);
        darkness_tilemap.color = new Color(0, 0, 0, 0);
        Invoke("Step3", 1);
    
        for (int i = 0; i < players.Count; i++)
        {
            players[i].position = room_positions[i];
        }
        cam.SetClampLeft(border_left);

        boss_hp_bar.SetActive(true);
    }

    private void Firebird_Fire()
    {
        if (!boss.IsLive()) return;

        if (darkness_step >= 4) Invoke("Firebird_Fire", Random.Range(5, 10));
        firebird.Fire();
    }

    private void Step3()
    {
        darkness_step = 3;
        SceneTransmitter.last_load_type = 1;
    }
}
