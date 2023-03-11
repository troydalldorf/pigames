namespace Core.Inputs;

public static class JoystickDirectionExtensions
{
    public static bool IsUp(this JoystickDirection @this)
    {
        return (@this & JoystickDirection.Up) > 0;
    }
    
    public static bool IsDown(this JoystickDirection @this)
    {
        return (@this & JoystickDirection.Down) > 0;
    }
    
    public static bool IsLeft(this JoystickDirection @this)
    {
        return (@this & JoystickDirection.Left) > 0;
    }
    
    public static bool IsRight(this JoystickDirection @this)
    {
        return (@this & JoystickDirection.Right) > 0;
    }
}