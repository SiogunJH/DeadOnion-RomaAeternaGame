using System;

[Flags]
public enum GridEntityCategory
{
    None = 0,
    Ally = 1 << 0, // 1
    Enemy = 1 << 1, // 2
    Object = 1 << 2 // 4
}