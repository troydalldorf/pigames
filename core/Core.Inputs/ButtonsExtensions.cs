namespace Core.Inputs;

public static class ButtonsExtensions
{
    public static bool IsRedPushed(this Buttons @this)
    {
        return (@this & Buttons.Red) > 0;
    }
    
    public static bool IsGreenPushed(this Buttons @this)
    {
        return (@this & Buttons.Green) > 0;
    }
    
    public static bool IsBluePushed(this Buttons @this)
    {
        return (@this & Buttons.Blue) > 0;
    }
    
    public static bool IsYellowPushed(this Buttons @this)
    {
        return (@this & Buttons.Yellow) > 0;
    }
}