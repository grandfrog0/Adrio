using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseEntity : MonoBehaviour
{
    [Header("Start Parameters")]
    [SerializeField] private Types.EntityTypes type;
    [SerializeField] private float max_hp = 1;
    [SerializeField] private float damage_strength = 1;
    [Header("In-Game Parameters")]
    [SerializeField] private float hp;
    [Header("Additional")]
    public Movement movement;
    public AudioSource hurt_sound;
    [SerializeField] private List<SpriteRenderer> outlines;
    private List<Color> outlines_colors = new List<Color>();
    private bool hurt;
    private bool died;
    public float light_strength = 1.5f;
    [SerializeField] private bool disable_gameobject_on_start;
    [Header("Inventory")]
    public List<Item> items;
    public EntityPoints points;
    public bool can_pick_up_items;

    public void TakeDamage(float value, BaseEntity enemy, Transform from=null) //Ouch!
    {
        if (hurt) return;
        float value_after_items_used = ItemEffects.GetDamageToTake(this, value, enemy, from);
        float value_final = value_after_items_used;
        hp -= value_final;
        NormalizeParameters();

        hurt = true;

        if (hurt_sound) hurt_sound.Play();
        if (movement)
        {
            movement.StopMove();
            if (from == null) movement.Impulse((transform.position - enemy.transform.position).normalized*18);
            else movement.Impulse((transform.position - from.position).normalized*18);
        }

        Statistics.Add($"{GetEntityType().ToString()}_damage_taked", (int) value);
    }

    public void GiveDamage(BaseEntity enemy) //N-na!
    {
        enemy.TakeDamage(damage_strength, this);
        Statistics.Add($"{GetEntityType().ToString()}_damage_given", (int) damage_strength);
    }

    public void Kill(BaseEntity enemy, Transform from=null)
    {
        hp = 0;
        NormalizeParameters();
        hurt = true;
        if (hurt_sound) hurt_sound.Play();
        if (movement)
        {
            movement.StopMove();
            if (from == null) movement.Impulse((transform.position - enemy.transform.position).normalized*18);
            else movement.Impulse((transform.position - from.position).normalized*18);
        }

        Statistics.Add($"{GetEntityType().ToString()}_died");
        Statistics.Add($"{enemy.GetEntityType().ToString()}_kills");
    }

    public bool IsLive()
    {
        return hp > 0;
    }

    public int GetHp()
    {
        return (int) hp;
    }

    public int GetDamage()
    {
        return (int) damage_strength;
    }

    public virtual void FixedUpdate()
    {
        if (hurt)
        {
            if (!IsLive()) return;
            for(int i = 0; i < outlines.Count; i++)
            {
                outlines[i].color = Color.Lerp(outlines[i].color, new Color(1, 0, 0, 1f), 0.4f);
            }
            if (outlines[0].color.r >= 0.9f && outlines[0].color.g <= 0.1f && outlines[0].color.b <= 0.1f) hurt = false;
        }
        else
        {
            for(int i = 0; i < outlines.Count; i++)
            {
                outlines[i].color = Color.Lerp(outlines[i].color, outlines_colors[i], 0.4f);
            }
        }
    }
    
    public virtual void Awake()
    {
        hp = max_hp;
        foreach(SpriteRenderer outline in outlines){outlines_colors.Add(outline.color);}
    }

    public virtual void Start()
    {
        if (disable_gameobject_on_start) gameObject.SetActive(false);
    }

    public virtual void NormalizeParameters()
    {
        if (hp < 0) 
        {
            hp = 0;
        }
        if (!IsLive())
        {
            for(int i = 0; i < outlines_colors.Count; i++) 
            {
                outlines_colors[i] = new Color(1, 0, 0, outlines_colors[i].a);
            }
            if (!died) OnDied();
        }
    }

    public virtual void OnDied()
    {
        return;
    }

    public Types.EntityTypes GetEntityType()
    {
        return type;
    }

    public virtual string GetItem(Item item, string point_name) //return OK or reason of stop
    {
        if (items.IndexOf(item) != -1) return "already_here";
        foreach (Item item_ in items) if (item.need_point == item_.need_point) return "occuped_point";

        items.Add(item);
        int point_index = (points && points.item_points_names.Count > 0) ? points.item_points_names.IndexOf(point_name) : -1;
        if (point_index == -1) item.transform.parent = this.transform;
        else item.transform.parent = points.item_points[point_index];
        item.PickUp(this);
        return "OK";
    }

    public bool HasItem(Item item)
    {
        return items.IndexOf(item) == -1 ? false : true;
    }

    public bool HasItemOfType(string item)
    {
        foreach(Item item_ in items) if (item_.naming == item) return true;
        return false;
    }

    public int GetItemIndexByName(string naming)
    {
        for(int i = 0; i < items.Count; i++)
        {
            if (items[i].naming == naming) return i;
        }
        return -1;
    }
}
