using System.Device.Gpio;
using System.Device.I2c;
using Core.Inputs.Seesaw;

namespace Core.Inputs;

public class Player : IDisposable
{
    private I2cDevice i2CDevice;
    private Attiny8X7SeeSaw seesaw;
    private const ulong Pin18Right = 1 << 18;
    private const ulong Pin19Left = 1 << 19;
    private const ulong Pin20down = 1 << 20;
    private const ulong Pin2Up = 1 << 2;
    public const ulong AllJoystickPins = Pin18Right | Pin19Left | Pin20down | Pin2Up;

    public Player(int joystickAddress)
    {
        i2CDevice = I2cDevice.Create(new I2cConnectionSettings(1,joystickAddress));
        seesaw = new Attiny8X7SeeSaw(i2CDevice);
        seesaw.SetGpioPinModeBulk(AllJoystickPins, PinMode.InputPullUp);
    }

    public JoystickDirection ReadJoystick()
    {
        var data = seesaw.ReadGpioDigitalBulk(AllJoystickPins);
        var result = JoystickDirection.None;
        if ((data & Pin18Right) == 0) result |= JoystickDirection.Right;
        if ((data & Pin19Left) == 0) result |= JoystickDirection.Left;
        if ((data & Pin20down) == 0) result |= JoystickDirection.Down;
        if ((data & Pin2Up) == 0) result |= JoystickDirection.Up;
        return result;
    }

    public void Dispose()
    {
        seesaw?.Dispose();
        seesaw = null!;
        i2CDevice?.Dispose();
        i2CDevice = null!;
    }
}