using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TranslateManager : MonoBehaviour, IInitializable
{
    [SerializeField] Dict<string, TMP_Text> translateObjects;

    //public List<string> names;
    //public List<string> texts;
    private int cur_scene = 0;

    private LanguageSection section;

    public void Check()
    {
        //objects.Clear();
        //object_names.Clear();
        //names.Clear();
        //texts.Clear();

        translateObjects = new();

        TMP_Text[] array = FindObjectsOfType<TMPro.TMP_Text>(true);
        foreach(TMP_Text obj in array)
        {
            if (obj == null) continue;
            translateObjects.Add(obj.name, obj);
        }

        Translator.Check();

        //all_names = Translator.GetAllKeys();
        //all_texts = Translator.GetAllValues();

        cur_scene = SceneTransmitter.GetActiveSceneIndex();
        section = Translator.GetScene(cur_scene);
        //int from_index = -1, to_index = all_names.Count;
        //for(int i = 0; i < all_names.Count; i++)
        //{
        //    if(all_names[i].Trim() == "[scene]")
        //    {
        //        if (all_texts[i].Trim() == cur_scene.ToString()) from_index = i + 1;
        //        else if (from_index != -1 && to_index == all_names.Count) to_index = i;
        //    } 
        //}
        //if (from_index == -1)
        //{
        //    // Debug.Log("translate scene index not found.");
        //    return; //EXIT
        //}
        //// else Debug.Log("Scene " + cur_scene + ": from " + from_index +  " to " + to_index);
        //for (int i = from_index; i < to_index; i++)
        //{
        //    if (i >= all_names.Count) break;
        //    names.Add(all_names[i]);
        //    texts.Add(all_texts[i]);
        //}

        foreach((string objName, TMP_Text objText) in translateObjects)
        {
            if (section.content.TryGetValue(objName, out string text))
                objText.text = text;
        }
        //for(int i = 0; i < names.Count; i++)
        //{
        //    if (object_names.IndexOf(names[i]) != -1) objects[object_names.IndexOf(names[i])].text = texts[i];
        //}
    }

    public InitializeOrder Order => InitializeOrder.TranslateManager;
    public void Initialize()
    {
        Check();
    }    
}
