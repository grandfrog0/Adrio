using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Types
{
    public enum EntityTypes {Non, Stuart, Jenny, Chokipie, DefaultMuddy, WalkingStone, BigStone, Firebird, BossKnight}
    public enum Families {Players=(0 << 8) | 3, Stones=(3 << 8) | 5}

    private static List<EntityTypes> family_array = new List<EntityTypes>(){EntityTypes.Stuart, EntityTypes.Jenny, EntityTypes.Chokipie, EntityTypes.WalkingStone, EntityTypes.BigStone};

    public static bool IsInFamily(EntityTypes type, Families family)
    {
        int index = family_array.IndexOf(type);
        if (index != -1)
        {
            if(index >= ((int) family >> 8) && index < (int) family - ((int) family >> 8))
            {
                return true;
            }
            else return false;
        }
        return false;
    }
}
