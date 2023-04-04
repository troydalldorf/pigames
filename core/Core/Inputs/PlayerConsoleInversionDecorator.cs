namespace Core.Inputs;

public class PlayerConsoleInversionDecorator : IPlayerConsole
{
    private readonly IPlayerConsole concrete;

    public PlayerConsoleInversionDecorator(IPlayerConsole concrete)
    {
        this.concrete = concrete;
    }

    public JoystickDirection ReadJoystick()
    {
        var stick = JoystickDirection.None;
        if (concrete.ReadJoystick().IsDown()) stick |= JoystickDirection.Up;
        if (concrete.ReadJoystick().IsUp()) stick |= JoystickDirection.Down;
        if (concrete.ReadJoystick().IsLeft()) stick |= JoystickDirection.Right;
        if (concrete.ReadJoystick().IsRight()) stick |= JoystickDirection.Left;
        return stick;
    }

    public Buttons ReadButtons()
    {
        return concrete.ReadButtons();
    }

    public void LightButtons(bool red, bool green, bool blue, bool yellow)
    {
        concrete.LightButtons(red, green, blue, yellow);
    }
}