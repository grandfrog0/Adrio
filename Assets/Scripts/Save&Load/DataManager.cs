using System;
using System.IO;
using TMPro;
using UnityEngine;
using Object = UnityEngine.Object;

public class DataManager : MonoBehaviour, IInitializable
{
    private const string _saveFileName = "save.json";
    private const string _statsFileName = "stats.json";
    public static string AppDataPath => Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Adrio");
    public static string SaveFilePath => Path.Combine(AppDataPath, _saveFileName);
    public static string StatsFilePath => Path.Combine(AppDataPath, _statsFileName);

    private static DataManager _inst;

    public static GameSave Game { get => _inst._game; private set => _inst._game = value; }
    public static Dict<string, int> Stats { get => _inst._stats; private set => _inst._stats = value; }

    private GameSave _game;
    private Dict<string, int> _stats;

    public InitializeOrder Order => InitializeOrder.DataManager;
    public void Initialize()
    {
        if (_inst == null)
            _inst = this;
    }

    public static void LoadAll()
    {
        if (!Directory.Exists(AppDataPath))
            Directory.CreateDirectory(AppDataPath);

        LoadGame();
        LoadStats();
        Debug.Log("Loaded.");
    }
    public static void SaveAll()
    {
        SaveGame();
        SaveStats();
        Debug.Log("Saved.");
    }

    public static void LoadGame()
    {
        try
        {
            string json = File.ReadAllText(SaveFilePath);
            Game = JsonUtility.FromJson<GameSave>(json);
        }
        catch (Exception e)
        {
            Debug.Log("Error on load game occured: " + e.Message);
            Game = new GameSave();
        }
    }
    public static void SaveGame()
    {
        string json = JsonUtility.ToJson(Game, true);
        File.WriteAllText(SaveFilePath, json);
    }
    public static void LoadStats()
    {
        try
        {
            string json = File.ReadAllText(StatsFilePath);
            Stats = Dict<string, int>.FromJson(json);
        }
        catch (Exception e)
        {
            Debug.Log("Error on load stats occured: " + e.Message);
            Stats = new();
        }
    }
    public static void SaveStats()
    {
        string json = Stats.ToJson(true);
        File.WriteAllText(StatsFilePath, json);
    }

    public static LanguageData GetLanguage(string language)
    {
        try
        {
            Object obj = Resources.Load("Language/" + language);
            return JsonUtility.FromJson<LanguageData>(obj.ToString());
        }
        catch (Exception e)
        {
            Debug.Log("error on language got occured: " + e.Message);
        }
        return new LanguageData();
    }
}
