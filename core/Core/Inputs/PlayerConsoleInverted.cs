namespace Core.Inputs;

public class PlayerConsoleInverted : PlayerConsole
{
    public PlayerConsoleInverted(int joystickAddress, int buttonsAddress) : base(joystickAddress, buttonsAddress)
    {
    }

    public override JoystickDirection ReadJoystick()
    {
        var stick = base.ReadJoystick();
        var invertedStick = JoystickDirection.None;
        if (stick.IsLeft()) invertedStick |= JoystickDirection.Right;
        if (stick.IsRight()) invertedStick |= JoystickDirection.Left;
        if (stick.IsUp()) invertedStick |= JoystickDirection.Down;
        if (stick.IsDown()) invertedStick |= JoystickDirection.Up;
        return invertedStick;
    }
}