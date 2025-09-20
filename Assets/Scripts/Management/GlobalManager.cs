using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalManager : MonoBehaviour, IInitializable
{
    private static bool inited = false;
    public InitializeOrder Order => InitializeOrder.GlobalManager;

    public void Initialize()
    {
        if (inited) return;

        Debug.Log("Global init");
        DataManager.LoadAll();
        inited = true;
    }

    private void OnApplicationQuit()
    {
        DataManager.SaveAll();
    }
    private void OnApplicationPause(bool pause)
    {
        DataManager.SaveAll();
    }
}
