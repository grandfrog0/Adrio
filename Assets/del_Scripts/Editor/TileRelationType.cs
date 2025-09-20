using System;

[Flags]
public enum TileRelationType
{
    None  = 0b0000,
    Left  = 0b0001,
    Right = 0b0010,
    Top   = 0b0100,
    Bottom= 0b1000,
    All   = 0b1111
}
