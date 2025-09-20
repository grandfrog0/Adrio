using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveItemTrigger : MonoBehaviour
{
    [SerializeField] private int item_trigger_index = -1;

    [SerializeField] private List<Transform> targets;
    [SerializeField] private GameObject item;
    [SerializeField] private float range = 2;
    [SerializeField] private bool target_in_range;
    [SerializeField] private bool used;

    [SerializeField] private AudioSource au;
    [SerializeField] private Transform tr;
    [SerializeField] private float to_scale = 1.25f;
    [SerializeField] private Rigidbody2D rb;

    private void FixedUpdate()
    {
        bool target_given = false;
        if (!used) foreach(Transform target in targets)
        {
            if (target.gameObject.activeSelf && Vector3.Distance(target.position, transform.position) <= range)
            {
                if (!target_in_range) 
                {
                    used = true;
                    if (au) au.Play();
                    Instantiate(item, target.position, Quaternion.identity);
                    rb.WakeUp();
                    rb.AddForce(new Vector2(Random.Range(-3f, 3f), 25), ForceMode2D.Impulse);

                    SaveItem();
                }
                target_in_range = true;
                target_given = true;
                break;
            }
        }
        if (!target_given)
        {
            target_in_range = false;
        }

        if (used)
        {
            if (tr)
            {
                if (to_scale > 0)
                {
                    if (tr.localScale.x < to_scale) tr.localScale += new Vector3(1, 1) * 10 * Time.fixedDeltaTime;
                    else to_scale = 0;
                }
                else
                {
                    if (tr.localScale.x > 0.1f) tr.localScale -= new Vector3(1, 1) * 5 * Time.fixedDeltaTime;
                    else Destroy(gameObject);
                }
            }
        }
    }

    private void SaveItem()
    {
        List<int> usedSaveItemTriggers = DataManager.Game.usedSaveItemTriggers;
        if (usedSaveItemTriggers.Contains(item_trigger_index))
        {
            Debug.Log("Item not saved: trigger has already been used.");
            return;
        }
        usedSaveItemTriggers.Add(item_trigger_index);
        DataManager.Game.usedSaveItemTriggers = usedSaveItemTriggers;

        EventMessageManager.Main().GetItem(item); //save and send message

        Statistics.Add("saved_items_count");

        Debug.Log("Item saved.");
    }

    private void Awake()
    {
        if (DataManager.Game.usedSaveItemTriggers.Contains(item_trigger_index)) 
            Destroy(gameObject);

        if (targets.Count == 0)
        {
            GameObject[] objects = GameObject.FindGameObjectsWithTag("Player");
            foreach(GameObject obj in objects) targets.Add(obj.transform);
        }
        rb.Sleep();
    }
 
    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position, range);
    }
}
