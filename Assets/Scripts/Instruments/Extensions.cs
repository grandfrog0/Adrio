using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public static class Extensions
{
    public static List<T> FindObjectsOfInterface<T>(this MonoBehaviour obj) where T : class
    {
        List<T> list = new List<T>();
        GameObject[] allGameObjects = GameObject.FindObjectsOfType<GameObject>();

        foreach (GameObject go in allGameObjects)
        {
            T[] components = go.GetComponents<T>();
            if (components != null && components.Length > 0)
            {
                list.AddRange(components);
            }
        }

        return list;
    }
    public static SortedSet<T> ToSortedSet<T>(this List<T> list, IComparer<T> comparer)
    {
        SortedSet<T> set = new(comparer);
        foreach (T t in list)
            set.Add(t);
        return set;
    }
}
