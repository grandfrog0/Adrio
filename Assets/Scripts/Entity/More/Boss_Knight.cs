using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss_Knight : BaseEntity
{
    [SerializeField] private GameObject knife, sword;
    [SerializeField] private Transform target;
    [SerializeField] private Vector3 border_lu, border_rb;
    [SerializeField] private Inputs inputs;
    [SerializeField] private bool game_started;
    [SerializeField] private float spawntime_min = 1f;
    [SerializeField] private float spawntime_max = 3f;
    [SerializeField] private Animator anim;
    [SerializeField] private CapsuleCollider2D capsule_coll;
    [SerializeField] private Vector2 died_size, died_offset;
    [SerializeField] private GameObject key;

    public void StartGame()
    {
        game_started = true;
        Invoke("Attack", Random.Range(spawntime_min, spawntime_max));
    }

    private void Attack()
    {
        if (game_started) Invoke("Attack", Random.Range(spawntime_min, spawntime_max));

        int type = Random.Range(0, 3);
        if (type < 2)
        {
            Knife obj = Instantiate(knife, new Vector3(Random.Range(border_lu.x, border_rb.x), Random.Range(border_rb.y, border_lu.y)), Quaternion.identity).GetComponent<Knife>();
            if (Mathf.Abs(obj.transform.position.x - border_lu.x) < 2) obj.transform.position = new Vector3(border_lu.x + Random.Range(-1f, 1f), obj.transform.position.y);
            else if (Mathf.Abs(obj.transform.position.x - border_rb.x) < 2) obj.transform.position = new Vector3(border_rb.x + Random.Range(-1f, 1f), obj.transform.position.y);
            else obj.transform.position = new Vector3(obj.transform.position.x, border_lu.y + Random.Range(-0.5f, 0.5f));
            
            if (target)
            {
                obj.Fire(this, GetDamage(), (target.position - obj.transform.position + new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f))).normalized);
                obj.SetEnemy(target);
            }
        }
        else if (type == 2)
        {
            if (target)
            {
                Sword obj = Instantiate(sword, new Vector3(target.position.x + Random.Range(-3f, 3f), target.position.y + Random.Range(-3f, 3f)), Quaternion.identity).GetComponent<Sword>();
                obj.Fire(this, GetDamage(), Vector2.zero);
            }
        }
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();

        target = inputs.GetCurPlayer();
        if (SceneTransmitter.need_scene != -1) game_started = false;
    }

    public override void OnDied()
    {
        base.OnDied();

        game_started = false;
        anim.Play("died");

        capsule_coll.size = died_size;
        capsule_coll.offset = died_offset;
        Instantiate(key, transform.position + Vector3.up * 1, Quaternion.identity);
    }
}
