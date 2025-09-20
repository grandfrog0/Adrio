using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tags : MonoBehaviour
{
    [SerializeField] private List<string> tags;

    public List<string> GetTags()
    {
        return tags;
    }

    public static bool HasTag(GameObject obj, string tag)
    {
        if (!obj.GetComponent<Tags>()) return false;
        else return obj.GetComponent<Tags>().GetTags().IndexOf(tag) != -1;
    }
}
