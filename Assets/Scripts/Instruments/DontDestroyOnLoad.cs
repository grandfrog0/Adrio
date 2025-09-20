using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DontDestroyOnLoad : MonoBehaviour
{
    private static List<string> occupiedTags = new();

    [SerializeField] string occupeTag;
    private void Awake()
    {
        if (occupiedTags.Contains(occupeTag))
        {
            Destroy(this);
            return;
        }

        DontDestroyOnLoad(this);
        occupiedTags.Add(occupeTag);
    }
}
