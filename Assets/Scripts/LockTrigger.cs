using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockTrigger : MonoBehaviour
{
    enum DataType {Nothing, ValueFloat, Text, ValueInt, Boolean, Transform};

    [SerializeField] private List<DataType> data_type;
    [SerializeField] private List<MonoBehaviour> obj;
    [SerializeField] private List<string> message;

    [SerializeField] List<float> value_float;
    [SerializeField] List<string> text;
    [SerializeField] List<int> value_int;
    [SerializeField] List<bool> boolean;
    [SerializeField] List<Transform> transform_;
    [SerializeField] List<GameObject> obj_;

    [SerializeField] private bool used;
    [SerializeField] private float range;
    [SerializeField] private Item need_item;
    [SerializeField] private string need_item_name;
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Sprite unlocked, unlocked_outline;
    [SerializeField] private SpriteRenderer sr, sr_outline;
    [SerializeField] private AudioSource au;

    private void FixedUpdate()
    {
        if (used) return;

        Collider2D[] colls = Physics2D.OverlapCircleAll(transform.position, range);
        foreach(Collider2D coll in colls)
        {
            if (coll.gameObject.GetComponent<BaseEntity>() && ((need_item != null && coll.gameObject.GetComponent<BaseEntity>().HasItem(need_item)) || coll.gameObject.GetComponent<BaseEntity>().GetItemIndexByName(need_item_name) != -1))
            {
                Use();

                rb.isKinematic = false;
                sr.sprite = unlocked;
                sr_outline.sprite = unlocked_outline;
                BaseEntity entity = coll.gameObject.GetComponent<BaseEntity>();
                Item item = need_item != null ? need_item : entity.items[entity.GetItemIndexByName(need_item_name)];
                item.Use();
                entity.items.Remove(item);
                if (au) au.Play();
            }
        }
    }

    private void Use()
    {
        if (used) return;
        used = true;
        for(int i = 0; i < obj.Count; i++)
        {
            switch (data_type[i])
            {
                case DataType.Nothing:
                    obj[i].SendMessage(message[i]);
                    break;

                case DataType.ValueFloat:
                    obj[i].SendMessage(message[i], value_float[i]);
                    break;
                        
                case DataType.Text:
                    obj[i].SendMessage(message[i], text[i]);
                    break;
                        
                case DataType.ValueInt:
                    obj[i].SendMessage(message[i], value_int[i]);
                    break;
                        
                case DataType.Boolean:
                    obj[i].SendMessage(message[i], boolean[i]);
                    break;
                        
                case DataType.Transform:
                    obj[i].SendMessage(message[i], transform_[i]);
                    break;
            }
        }
    }

    void Awake()
    {
        rb.isKinematic = true;
    }
    
    void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position, range);
    }
}
