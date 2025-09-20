using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class Translator
{
    private static string language;
    private static LanguageData languageData;
    //private static List<string>[] items = new List<string>[]{};
    //private static List<string> all_names = new List<string>();
    //private static List<string> all_texts = new List<string>();

    public static LanguageSection GetSection(string name)
    {
        if (languageData?.sections.TryGetValue(name, out LanguageSection section) == true)
            return section;
        return new();
    }
    public static LanguageSection GetScene(int index)
    {
        if (languageData?.scenes.TryGetValue(index, out LanguageSection section) == true)
            return section;
        return new();
    }
    /*public static List<string> GetSection(string section, bool values)
    {
        if (languageData == null) 
            Check();
        CheckLanguage();

        int from_index = -1, to_index = all_names.Count;
        for(int i = 0; i < all_names.Count; i++)
        {
            if(all_names[i].Trim() == "[section]")
            {
                if (all_texts[i].Trim() == section.Trim()) from_index = i + 1;
                else if (from_index != -1 && to_index == all_names.Count) to_index = i;
            } 
            else if (all_names[i].Trim().StartsWith("["))
            {
                if (from_index != -1 && to_index == all_names.Count) to_index = i;
            }
        }
        if (from_index == -1)
        {
            Debug.Log("section not found.");
            return null; //EXIT
        }
        List<string> k = new List<string>();
        List<string> v = new List<string>();
        for (int i = from_index; i < to_index; i++)
        {
            if (i >= all_names.Count) break;
            k.Add(all_names[i]);
            v.Add(all_texts[i]);
        }

        return values ? v : k;
    }*/

    public static string GetValue(string sectionName, string key)
    {
        LanguageSection section = GetSection(sectionName);
        if (section.content.TryGetValue(key, out string res))
            return res;
        return "";
    }
    //{
    //    List<string> k = GetSection(section, false);
    //    List<string> v = GetSection(section, true);
    //    return k.IndexOf(key) != -1 ? v[k.IndexOf(key)] : "";
    //}
    
    public static void CheckLanguage()
    {
        language = DataManager.Game.language != "" ? DataManager.Game.language : "en_en";
    }

    public static string GetLanguage()
    {
        CheckLanguage();
        return language;
    }

    //public static List<string> GetAllKeys()
    //{
    //    if (languageData == null) Check();

    //    return languageData.sections.;
    //}

    //public static List<string> GetAllValues()
    //{
    //    if (languageData.Length == 0) Check();
        
    //    return all_texts;
    //}

    public static void Check()
    {
        CheckLanguage();

        // items = DataManager.GetFileItems("Language/" + Translator.language + ".hat", true);

        // REMAKE IT
        languageData = DataManager.GetLanguage(language);
        Debug.Log(languageData);
        //all_names = languageData[0];
        //all_texts = languageData[1];
    }
}
