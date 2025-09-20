using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Basketball : MonoBehaviour
{
    public bool up_collisioned, down_collisioned, ignore_next_reward, rewarded;
    public ParticleSystem particles;
    public AudioSource au;

    enum DataType {Nothing, ValueFloat, Text, ValueInt, Boolean, Transform};

    [SerializeField] private List<DataType> data_type;
    [SerializeField] private List<MonoBehaviour> obj;
    [SerializeField] private List<string> message;

    [SerializeField] List<float> value_float;
    [SerializeField] List<string> text;
    [SerializeField] List<int> value_int;
    [SerializeField] List<bool> boolean;
    [SerializeField] List<Transform> transform_;
    [SerializeField] List<GameObject> obj_;

    public void Collisioned(bool up)
    {
        if (up)
        {
            if (!down_collisioned) up_collisioned = true;
            else
            {
                ClearAll();
                ignore_next_reward = true;
                Invoke("not_ignore_next_reward", 4);
            }
        }
        if (!up)
        {
            down_collisioned = true;
            if (up_collisioned)
            {
                if (!ignore_next_reward) Reward();
                else
                {
                    ClearAll();
                    ignore_next_reward = false;
                }
            }
        }
    }

    private void not_ignore_next_reward()
    {
        ignore_next_reward = false;
        ClearAll();
    }

    private void ClearAll()
    {
        down_collisioned = false;
        up_collisioned = false;
    }

    private void Reward()
    {
        ClearAll();
        // Debug.Log("GoodJob!");
        if (au) au.Play();
        particles.Play();
        if (!rewarded) Complete();
    }

    private void Complete()
    {
        if (rewarded) return;
        rewarded = true;
        for(int i = 0; i < obj.Count; i++)
        {
            switch (data_type[i])
            {
                case DataType.Nothing:
                    obj[i].SendMessage(message[i]);
                    break;

                case DataType.ValueFloat:
                    obj[i].SendMessage(message[i], value_float[i]);
                    break;
                        
                case DataType.Text:
                    obj[i].SendMessage(message[i], text[i]);
                    break;
                        
                case DataType.ValueInt:
                    obj[i].SendMessage(message[i], value_int[i]);
                    break;
                        
                case DataType.Boolean:
                    obj[i].SendMessage(message[i], boolean[i]);
                    break;
                        
                case DataType.Transform:
                    obj[i].SendMessage(message[i], transform_[i]);
                    break;
            }
        }
    }
}
