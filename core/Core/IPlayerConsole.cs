namespace Core;

public interface IPlayerConsole
{
    JoystickDirection ReadJoystick();
    Buttons ReadButtons();
    void LightButtons(bool red, bool green, bool blue, bool yellow);
}