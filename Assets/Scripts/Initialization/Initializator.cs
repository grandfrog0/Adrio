using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class Initializator : MonoBehaviour
{
    public void Awake()
    {
        SortedSet<IInitializable> initializables = this.FindObjectsOfInterface<IInitializable>().ToSortedSet(new InitComparer());

        foreach (IInitializable init in initializables)
        {
            init.Initialize();
        }
    }

    public static void InitObject(GameObject obj, List<IInitializable> ignore)
    {
        SortedSet<IInitializable> initializables = obj.
            GetComponentsInChildren<MonoBehaviour>().OfType<IInitializable>().Where(x => !ignore.Contains(x)).
            ToList().ToSortedSet(new InitComparer());

        foreach (IInitializable init in initializables)
            init.Initialize();
    }
    public static void InitObject(GameObject obj)
    {
        SortedSet<IInitializable> initializables = obj.
            GetComponentsInChildren<MonoBehaviour>().OfType<IInitializable>().
            ToList().ToSortedSet(new InitComparer());

        foreach (IInitializable init in initializables)
            init.Initialize();
    }
}
