using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikyObject : BaseEntity
{
    public List<Types.EntityTypes> entity_types;

    void OnTriggerEnter2D(Collider2D coll)
    {
        if (coll.gameObject.GetComponent<BaseEntity>() && coll.gameObject != this.gameObject)
        {
            BaseEntity entity = coll.gameObject.GetComponent<BaseEntity>();
            if (entity_types.IndexOf(entity.GetEntityType()) >= 0) entity.GiveDamage(entity);
        }
    }
}
