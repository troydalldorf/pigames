namespace Core;

[Flags]
public enum JoystickDirection
{
    None        = 0x00,
    Up          = 0x01,
    Down        = 0x02,
    Left        = 0x04,
    Right       = 0x08,
    UpAndLeft   = Up | Left,
    UpAndRight  = Up | Right,
    DownAndLeft = Down | Left,
    DownAndRight= Down | Right,
}