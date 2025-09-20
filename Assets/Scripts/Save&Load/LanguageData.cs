using System;
using System.Collections.Generic;
using UnityEngine.UIElements;

[Serializable]
public class LanguageData
{
    public string name;
    public Dict<string, LanguageSection> sections = new();
    public Dict<int, LanguageSection> scenes = new();

    public override string ToString()
        => $"{name}: sections: [{string.Join(";", sections)}], scenes: [{string.Join(";", scenes)}].";
}
