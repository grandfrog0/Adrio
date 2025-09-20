using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventMessageObject
{
    public EventMessageManager.EventMessageType type;
    public string text;
    public GameObject spawn_object;
    public Sprite icon;

    public EventMessageObject(EventMessageManager.EventMessageType type, GameObject spawn_object)
    {
        this.type = type;
        this.spawn_object = spawn_object;
        Debug.Log(type);
    }

    public EventMessageObject(EventMessageManager.EventMessageType type, string text, Sprite icon)
    {
        this.type = type;
        this.text = text;
        this.icon = icon;
        Debug.Log(type);
    }
}
