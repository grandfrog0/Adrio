using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChokipieEntity : BaseEntity
{
    public override string GetItem(Item item, string point_name)
    {
        string result = base.GetItem(item, point_name);
        if (point_name == "head_top") for(int i = 0; i < items.Count; i++) if (items[i].naming == "hat") 
        {
            if (items[i].gameObject.activeSelf && result == "occuped_point") 
            {   
                items.Add(item);
                int point_index = (points && points.item_points_names.Count > 0) ? points.item_points_names.IndexOf(point_name) : -1;
                if (point_index == -1) item.transform.parent = this.transform;
                else item.transform.parent = points.item_points[point_index];
                item.PickUp(this);
                result = "OK";
            }
            items[i].gameObject.SetActive(false);
        }

        return result;
    }
}
