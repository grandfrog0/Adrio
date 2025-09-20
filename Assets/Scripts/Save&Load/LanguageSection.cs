using System;

[Serializable]
public class LanguageSection
{
    public Dict<string, string> content = new();
    public override string ToString()
    {
        return string.Join(", ", content);
    }
}
