using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EyeBlink : MonoBehaviour
{
    [SerializeField] List<Transform> eyes;
    [SerializeField] Vector2 timeBorder = new Vector2(5, 8);
    private List<Vector3> startScales = new List<Vector3>();
    private float blinkTime = 0.2f;

    private void Start()
    {
        foreach(Transform eye in eyes)
        {
            startScales.Add(eye.localScale);
        }
        StartCoroutine(Blink());
    }

    private IEnumerator Blink()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(timeBorder[0], timeBorder[1]));

            float timeLeft = 0f;
            while (timeLeft < blinkTime)
            {
                timeLeft += Time.deltaTime;

                for (int i = 0; i < eyes.Count; i++)
                    eyes[i].transform.localScale = Vector3.Lerp(startScales[i], new Vector3(startScales[i].x, 0, 1), timeLeft / blinkTime);
                yield return null;
            }

            timeLeft = 0f;
            while (timeLeft < blinkTime)
            {
                timeLeft += Time.deltaTime;

                for (int i = 0; i < eyes.Count; i++)
                    eyes[i].transform.localScale = Vector3.Lerp(new Vector3(startScales[i].x, 0, 1), startScales[i], timeLeft / blinkTime);

                yield return null;
            }
        }
    }
}
