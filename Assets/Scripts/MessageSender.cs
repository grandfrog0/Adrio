using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MessageSender : MonoBehaviour
{
    enum IfUsed {Destroy, Disable, TriggerMessage, MessageEveryTick};
    enum DataType {Nothing, ValueFloat, Text, ValueInt, Boolean, Transform, GameObject};

    [SerializeField] private IfUsed if_used;
    [SerializeField] private DataType data_type;
    [SerializeField] private MonoBehaviour obj;
    [SerializeField] private List<Transform> targets;
    [SerializeField] private string message;
    [SerializeField] private float range;
    [SerializeField] private bool target_in_range;

    [SerializeField] float value_float;
    [SerializeField] string text;
    [SerializeField] int value_int;
    [SerializeField] bool boolean;
    [SerializeField] Transform transform_;
    [SerializeField] GameObject obj_;

    private void FixedUpdate()
    {
        bool target_given = false;
        foreach(Transform target in targets)
        {
            if (target.gameObject.activeSelf && Vector3.Distance(target.position, transform.position) <= range)
            {
                if (!target_in_range && if_used == IfUsed.TriggerMessage) Send();
                if (!target_in_range && if_used == IfUsed.Destroy) 
                {
                    Send();
                    Destroy(gameObject);
                }
                else if (!target_in_range && if_used == IfUsed.Disable) 
                {
                    Send();
                    gameObject.SetActive(false);
                }
                target_in_range = true;
                target_given = true;
                break;
            }
        }
        if (!target_given)
        {
            target_in_range = false;
        }

        if (target_in_range && if_used == IfUsed.MessageEveryTick) Send();
    }

    private void Send()
    {
        if (!obj) return;
        switch (data_type)
        {
            case DataType.Nothing:
                obj.SendMessage(message);
                break;

            case DataType.ValueFloat:
                obj.SendMessage(message, value_float);
                break;
                
            case DataType.Text:
                obj.SendMessage(message, text);
                break;
                
            case DataType.ValueInt:
                obj.SendMessage(message, value_int);
                break;
                
            case DataType.Boolean:
                obj.SendMessage(message, boolean);
                break;
                
            case DataType.Transform:
                obj.SendMessage(message, transform_);
                break;
                
            case DataType.GameObject:
                obj.SendMessage(message, obj_);
                break;
        }
        
    }

    private void Awake()
    {
        if (targets.Count == 0)
        {
            GameObject[] objects = GameObject.FindGameObjectsWithTag("Player");
            foreach(GameObject obj in objects) targets.Add(obj.transform);
        }
    }
 
    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position, range);
    }
}
