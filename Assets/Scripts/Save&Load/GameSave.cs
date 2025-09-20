using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class GameSave
{
    public List<int> openedLevels;
    public List<int> completedLevels;
    public List<string> completedAchievements;
    public List<ItemInfo> savedItems;
    public string language;
    public string menuMusic;
    public bool isSoundMuted;
    public bool isMusicMuted;
    public List<int> usedSaveItemTriggers;

    public GameSave()
    {
        openedLevels = new();
        completedLevels = new();
        completedAchievements = new();
        savedItems = new();
        language = "";
        menuMusic = "";
        isSoundMuted = false;
        isMusicMuted = false;
        usedSaveItemTriggers = new();
    }

    public override string ToString()
        => $"Opened levels: {string.Join(", ", openedLevels)}, completed: {string.Join(", ", completedLevels)}, " +
            $"achievements: {string.Join(", ", completedAchievements)}, items: {string.Join(", ", savedItems)}, " +
            $"language: {language}, menu music: {menuMusic}, sound muted: {isSoundMuted}, music muted: {isMusicMuted}";
}
