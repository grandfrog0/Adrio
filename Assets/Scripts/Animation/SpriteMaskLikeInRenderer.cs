using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteMaskLikeInRenderer : MonoBehaviour
{
    [SerializeField] SpriteMask mask;
    [SerializeField] SpriteRenderer rendererForSprite;
    [SerializeField] SpriteRenderer rendererForLayer;
    [SerializeField] int frontSortingOrder, backSortingOrder;

    void Update()
    {
        if (rendererForSprite && mask.sprite != rendererForLayer.sprite) 
            mask.sprite = rendererForSprite.sprite;
        if (rendererForLayer)
        {
            mask.backSortingOrder = rendererForSprite.sortingOrder + backSortingOrder;
            mask.frontSortingOrder = rendererForLayer.sortingOrder + frontSortingOrder;
        }
    }
}
