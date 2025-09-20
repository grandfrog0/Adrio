using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadSceneBlockAnimation : MonoBehaviour
{
    [SerializeField] List<SpriteRenderer> topBlockRenderer;
    [SerializeField] List<SpriteRenderer> anotherBlockRenderer;
    [SerializeField] List<SpriteRenderer> decorateRenderer;

    [SerializeField] List<LoadSceneBlockAnimationConfig> configs;
    [SerializeField] List<int> sceneBorders;
    private int curLocation = -1;

    public void Check()
    {
        int needScene = SceneTransmitter.need_scene >= 0 ? SceneTransmitter.need_scene : SceneTransmitter.GetActiveSceneIndex();
        if (SceneTransmitter.need_scene >= SceneManager.sceneCountInBuildSettings) 
            needScene = 0;

        int needLocation = 0;
        for(int i = 0; i < sceneBorders.Count; i++) 
            if (needScene < sceneBorders[i])
            {
                needLocation = i;
                if (needLocation != curLocation)
                {
                    SetLocation(needLocation);
                }
                break;
            }
    }

    void Awake()
    {
        Check();
    }

    private void SetLocation(int cur)
    {
        curLocation = cur;

        foreach(SpriteRenderer sr in topBlockRenderer) 
            sr.sprite = configs[cur].topBlockSprite;
            
        foreach(SpriteRenderer sr in anotherBlockRenderer) 
            sr.sprite = configs[cur].anotherBlockSprite;

        for(int i = 0; i < decorateRenderer.Count; i++) 
            decorateRenderer[i].sprite = configs[cur].decorateSprites[i];
    }
}
