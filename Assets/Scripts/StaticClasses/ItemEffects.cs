using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ItemEffects
{
    private static int pocket_stone_use_chance = 50;

    public static float GetDamageToTake(BaseEntity entity, float value, BaseEntity enemy, Transform from=null)
    {
        float new_value = value;
        Types.EntityTypes enemy_type = enemy.GetEntityType();
        if (Types.IsInFamily(enemy_type, Types.Families.Stones))
        {
            if (entity.HasItemOfType("pocket_stone"))
            {
                new_value = MyMath.Chance(pocket_stone_use_chance) ? 0f : new_value;
            }
        }
        // if (entity.HasItemOfType("clown_nose"))
        // {
        //     if (enemy.movement)
        //     {
        //         enemy.movement.StopMove();
        //         if (from == null) enemy.movement.Impulse((enemy.transform.position - entity.transform.position).normalized*4);
        //         else enemy.movement.Impulse((from.position + entity.transform.position).normalized*4);
        //     }
        // }
        return new_value;
    }
}
