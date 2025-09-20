using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    public string naming = "";
    public string need_point = "";
    public bool picked_up;
    [SerializeField] private SineTranslate sine;
    public List<SpriteRenderer> renderers;
    public List<int> start_layers;
    public BaseEntity parent;
    public List<KeyCode> use_keycode;

    public virtual void OnTriggerEnter2D(Collider2D coll)
    {
        if (coll.gameObject.GetComponent<BaseEntity>() && !picked_up)
        {
            BaseEntity entity = coll.gameObject.GetComponent<BaseEntity>();
            if (entity.can_pick_up_items)
            {
                entity.GetItem(this, need_point);
                if (sine)
                {
                    if (picked_up) sine.Reset();
                    sine.IsActive = !picked_up;
                }
            }
        }
    }

    public virtual void Use()
    {
        Destroy(gameObject);
    }

    public void SetSortingOrder(int value=0)
    {
        for(int i = 0; i < renderers.Count; i++)
        {
            renderers[i].sortingOrder = start_layers[i] + value;
        }
    }

    public void SetSortingLayer(int add_value, string naming)
    {
        for(int i = 0; i < renderers.Count; i++)
        {
            renderers[i].sortingOrder += add_value;
            renderers[i].sortingLayerName = naming;
        }
    }

    public virtual void PickUp(BaseEntity entity=null)
    {
        OnlyPickUp(entity);
    }

    public virtual void OnlyPickUp(BaseEntity entity=null)
    {
        picked_up = true;
        
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.identity;
        transform.localScale = new Vector3(1, 1, 1);

        if (entity) parent = entity;

        if (sine)
        {
            sine.Reset();
            sine.IsActive = false;
        }
    }

    private void Awake()
    {
        start_layers.Clear();
        foreach(SpriteRenderer sr in renderers) start_layers.Add(sr.sortingOrder);
    }
}
