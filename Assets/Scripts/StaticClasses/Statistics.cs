using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Statistics
{
    private static Dict<string, int> items = new();

    public static void Add(string name, int value=1, bool save_later=false)
    {
        if (!items.ContainsKey(name))
            DataManager.Stats[name] = 0;
        else
            DataManager.Stats[name] += value;
    }
}
