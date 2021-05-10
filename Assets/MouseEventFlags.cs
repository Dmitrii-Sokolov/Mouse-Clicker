using System;

[Flags]
public enum MouseEventFlags
{
    Move = 0x00000001,
    LeftDown = 0x00000002,
    LeftUp = 0x00000004,
    RightDown = 0x00000008,
    RightUp = 0x00000010,
    MiddleDown = 0x00000020,
    MiddleUp = 0x00000040,
    Absolute = 0x00008000
}