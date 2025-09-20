using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInfo : MonoBehaviour
{
    [SerializeField] private Inputs inputs;
    [SerializeField] private Movement movement;
    [SerializeField] private int index;
    [SerializeField] private Sprite hp_bar_image;
    [SerializeField] private int value;

    public void ReplacePlayer()
    {
        inputs.ReplacePlayer(movement, index, hp_bar_image, value);
    }
}
