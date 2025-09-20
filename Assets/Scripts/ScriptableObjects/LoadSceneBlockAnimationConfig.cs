using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName="Blocks1", menuName="Scriptable Objects/LoadSceneBlockAnimation")]
public class LoadSceneBlockAnimationConfig : ScriptableObject
{
    public Sprite topBlockSprite;
    public Sprite anotherBlockSprite;
    public List<Sprite> decorateSprites;
}
