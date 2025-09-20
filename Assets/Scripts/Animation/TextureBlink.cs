using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextureBlink : MonoBehaviour
{
    [SerializeField] SpriteRenderer sr;
    [SerializeField] Vector2 timeBorder = new Vector2(5, 8);
    [SerializeField] List<Sprite> sprites;
    private float blink_time = 0.2f;

    private void Start()
    {
        sr.sprite = sprites[0];
        StartCoroutine(Blink());
    }

    private IEnumerator Blink()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(timeBorder[0], timeBorder[1]));

            if (sr.sprite != sprites[1]) 
                sr.sprite = sprites[1];

            yield return new WaitForSeconds(blink_time);

            if (sr.sprite != sprites[0]) 
                sr.sprite = sprites[0];
        }
    }
}
