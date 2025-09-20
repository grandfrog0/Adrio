using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleSpriteAnimation : MonoBehaviour
{
    [SerializeField] SpriteRenderer sr;
    [SerializeField] List<Sprite> sprites;
    [SerializeField] float speed = 4;
    [SerializeField] SimpleSpriteAnimation bringCurFrom;
    private int cur;
    private float speedMultiplier = 1;

    public int CurrentFrame => cur;
    public float SpeedMultiplier
    {
        get { return speedMultiplier; }
        set { speedMultiplier = value; }
    }

    void Start()
    {
        if (sprites.Count > 0) 
            sr.sprite = sprites[cur]; 

        if (bringCurFrom == null && (sprites.Count > 1 && speed * speedMultiplier > 0))
            StartCoroutine(ChangeSprite());
    }

    void FixedUpdate()
    {
        if (bringCurFrom != null)
        {
            cur = bringCurFrom.CurrentFrame;
            sr.sprite = sprites[cur];
        }
    }

    IEnumerator ChangeSprite()
    {
        while (speed * speedMultiplier > 0 && sprites.Count > 0)
        {
            yield return new WaitForSeconds(1 / (speed * speedMultiplier));

            cur = ++cur % sprites.Count;
            sr.sprite = sprites[cur];
        }
    }
}
