using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using System.Linq;

public static class DataManager_Old_
{
    private static string appDataPath = "";
    // private static string assetDataPath = "";
    private static string fileName = "chokipie.hat";
    private static string filePath = "";

    private static List<string> texts, keys, values, lines;

    private static string save_name = "player";

    public static void SetValue(string key, string value)
    {
        Prepare();
        
        if (keys.IndexOf(key.Trim()) != -1) values[keys.IndexOf(key.Trim())] = value.Trim();
        else
        {
            keys.Add(key.Trim());
            values.Add(value.Trim());
        }
        for(int i = 0; i < keys.Count; i++)
        {
            lines.Add(keys[i] + "=" + values[i]);
        }
        File.WriteAllLines(filePath, lines);
    }
    public static string GetValue(string key)
    {
        Prepare();

        if (keys.IndexOf(key.Trim()) == -1) return "";
        else return values[keys.IndexOf(key.Trim())];
    }
    public static bool IsValue(string key, string value="1")
    {
        return GetValue(key.Trim()) == value.Trim();
    }
    public static bool HasValue(string key)
    {
        Prepare();
        return keys.IndexOf(key.Trim()) != -1;
    }
    public static void SetList(string key, List<string> vals)
    {
        SetValue(key, string.Join(", ", vals));
    }
    public static List<string> GetList(string key)
    {
        string text = GetValue(key.Trim());
        string[] vals = text.Split(",");
        List<string> list = new List<string>();
        foreach(string val in vals) if(val.Trim() != "") list.Add(val.Trim());
        return list;
    }
    public static void SetDict(string key, List<List<string>> steps)
    {
        List<string> vals = new List<string>();
        if (steps.Count > 0)
        for(int i = 0; i < steps[0].Count; i++) //all keys -> all values -> step3..
        {
            List<string> step_ = new List<string>();
            for(int step = 0; step < steps.Count; step++) //all collumns
            {
                step_.Add(steps[step][i]);
            }
            vals.Add(string.Join(":", step_).Trim());
        }
        SetValue(key, string.Join(", ", vals));
    }
    public static void SetDict(string key, List<string> ks, List<string> vs)
    {
        List<List<string>> steps = new List<List<string>>{ks, vs};
        SetDict(key, steps);
    }
    public static List<string> GetDictStep(string key, int index)
    {
        List<string> list = GetList(key.Trim());
        List<string> ks = new List<string>();
        foreach(string text in list)
        {
            if (index > text.Split(":").Length - 1) return ks;
            ks.Add(text.Split(":")[index].Trim());
        }
        return ks;
    }
    public static List<string> GetDictKeys(string key)
    {
        return GetDictStep(key.Trim(), 0);
    }
    public static List<string> GetDictValues(string key)
    {
        return GetDictStep(key.Trim(), 1);
    }
    //TECHNIC PART
    public static void SetFileItems(string file_name, List<string> ks, List<string> vs)
    {
        string file_path = Path.Combine(appDataPath, file_name);
        List<string> texts_ = GetFileByStrings(file_path);
        List<string> lines_ = new List<string>();
        for(int i = 0; i < ks.Count; i++)
        {
            lines_.Add(ks[i] + "=" + vs[i]);
        }
        File.WriteAllLines(file_path, lines_);
    }
    public static List<string>[] GetFileItems(string file_name, bool resourses=false)
    {

        List<string> keys_ = new List<string>();
        List<string> values_ = new List<string>();

        if (resourses) // из папки Resources
        {
            // TextAsset textAsset = Resources.Load<TextAsset>(file_name);
            // if (textAsset != null)
            // {
            //     Debug.Log(textAsset.text);
            // }
            // else Debug.Log("Asset not found :(");

            UnityEngine.Object[] objectsInPath = Resources.LoadAll("");
            //foreach (var obj in objectsInPath)
            //{
            //    Debug.Log($"Объект в пути Language: {obj.name}");
            //}
        }
        else // из папки AppData
        {
            string file_path = "";
            file_path = Path.Combine(appDataPath, file_name);
            // if (!resourses) ... else file_path = Path.Combine(assetDataPath, file_name);
            List<string> texts_ = GetFileByStrings(file_path);
            if (texts_ == null)
            {
                File.WriteAllLines(file_path, new List<string>());
                texts_ = GetFileByStrings(file_path);
            }
            // here were keys_ and values_.
            foreach(string text in texts_)
            {
                string[] splited = text.Split("=");
                keys_.Add(splited[0]);
                string[] splited_vals = new string[splited.Length - 1];
                Array.Copy(splited, 1, splited_vals, 0, splited.Length - 1);
                values_.Add(string.Join("=", splited_vals));
            }
        }
        return new List<string>[]{keys_, values_};
    }
    public static List<string> GetFileByStrings(string file_path)
    {
        if (!File.Exists(file_path)) return new List<string>();
        return File.ReadAllLines(file_path).ToList();
    }
    public static string GetAppDataFilePath()
    {
        return Path.Combine(appDataPath, fileName);
    }
    public static string GetResourcesPath()
    {
        return Path.Combine(Application.dataPath, "Resources");
    }
    private static void Prepare()
    {
        if (appDataPath == "")
        {
            appDataPath = Path.Combine(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Adrio"), save_name);
            Directory.CreateDirectory(appDataPath);
        }
        // if (assetDataPath == "") assetDataPath = GetResourcesPath();
        if (filePath == "") filePath = GetAppDataFilePath();

        texts = GetFileByStrings(filePath);
        keys = new List<string>();
        values = new List<string>();
        lines = new List<string>();
        foreach(string text in texts)
        {
            string[] splited = text.Split("=");
            if (splited.Length != 2) continue;
            keys.Add(splited[0]);
            values.Add(splited[1]);
        }
    }
}
