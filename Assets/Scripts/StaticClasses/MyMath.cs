using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class MyMath
{
    public static bool Chance(float value)
    {
        return Random.Range(0f, 100f) < value;
    }

    public static int Int(string value)
    {
        return int.Parse(value);
    }
    
    public static int Int(bool value)
    {
        return value ? 1 : 0;
    }

    public static int Int(float value)
    {
        return (int) value;
    }

    public static float Sum(List<float> values)
    {
        float sum = 0;
        foreach(float value in values) sum += value;
        return sum;
    }

    public static int Sum(List<int> values)
    {
        int sum = 0;
        foreach(int value in values) sum += value;
        return sum;
    }

    public static int Min(List<int> values, int else_value=0)
    {
        if (values.Count == 0) return else_value;
        return values.Min();
    }

    public static int Max(List<int> values, int else_value=0)
    {
        if (values.Count == 0) return else_value;
        return values.Max();
    }

    public static List<int> Sorted(List<int> list_int)
    {
        List<int> list = new List<int>(list_int);
        list.Sort();
        return list;
    }

    public static List<int> ListStrToInt(List<string> list_str)
    {
        List<int> list = new List<int>();
        foreach(string str in list_str) if (str != "" && str != null) list.Add(Int(str));
        return list;
    }

    public static List<string> ListIntToStr(List<int> list_int)
    {
        List<string> list = new List<string>();
        foreach(int val in list_int) list.Add(val.ToString());
        return list;
    }

    public static PlayerType ToPlayerType(Types.EntityTypes entityType)
        => entityType switch
        {
            Types.EntityTypes.Stuart => PlayerType.Stuart,
            Types.EntityTypes.Jenny => PlayerType.Jenny,
            Types.EntityTypes.Chokipie => PlayerType.Chokipie,
            _ => PlayerType.None
        };
}
