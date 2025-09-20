using System;

[Serializable]
public class ItemInfo
{
    public string name;
    public PlayerType player;

    public ItemInfo(string name, PlayerType player = PlayerType.None)
    {
        this.name = name;
        this.player = player;
    }

    public override string ToString() => $"{name}: {player}";
}
