using System.Device.Gpio;
using System.Device.I2c;
using Core.Inputs.Seesaw;

namespace Core.Inputs;

public class Player : IDisposable
{
    private const int BusId = 1;
    private I2cDevice joystickDevice;
    private Attiny8X7SeeSaw joystickSeesaw;
    private I2cDevice buttonsDevice;
    private Attiny8X7SeeSaw buttonsSeesaw;
    private const ulong Pin18Right = 1 << 18;
    private const ulong Pin19Left = 1 << 19;
    private const ulong Pin20down = 1 << 20;
    private const ulong Pin2Up = 1 << 2;
    public const ulong AllJoystickPins = Pin18Right | Pin19Left | Pin20down | Pin2Up;
    private const ulong Pin18A = 1 << 18;
    private const ulong Pin19B = 1 << 19;
    private const ulong Pin20X = 1 << 20;
    public const ulong AllButtonPins = Pin18A | Pin19B | Pin20X;

    public Player(int joystickAddress, int buttonsAddress)
    {
        joystickDevice = I2cDevice.Create(new I2cConnectionSettings(BusId, joystickAddress));
        joystickSeesaw = new Attiny8X7SeeSaw(joystickDevice);
        joystickSeesaw.SetGpioPinModeBulk(AllJoystickPins, PinMode.InputPullUp);
        
        buttonsDevice = I2cDevice.Create(new I2cConnectionSettings(BusId, buttonsAddress));
        buttonsSeesaw = new Attiny8X7SeeSaw(buttonsDevice);
        buttonsSeesaw.SetGpioPinModeBulk(AllButtonPins, PinMode.InputPullUp);
    }

    public JoystickDirection ReadJoystick()
    {
        var data = joystickSeesaw.ReadGpioDigitalBulk(AllJoystickPins);
        var result = JoystickDirection.None;
        if ((data & Pin18Right) == 0) result |= JoystickDirection.Right;
        if ((data & Pin19Left) == 0) result |= JoystickDirection.Left;
        if ((data & Pin20down) == 0) result |= JoystickDirection.Down;
        if ((data & Pin2Up) == 0) result |= JoystickDirection.Up;
        return result;
    }
    
    public Button ReadButtons()
    {
        var data = buttonsSeesaw.ReadGpioDigitalBulk(AllButtonPins);
        var result = Button.None;
        if ((data & Pin18A) == 0) result |= Button.A;
        if ((data & Pin19B) == 0) result |= Button.B;
        if ((data & Pin20X) == 0) result |= Button.X;
        return result;
    }

    public void Dispose()
    {
        joystickSeesaw?.Dispose();
        joystickSeesaw = null!;
        joystickDevice?.Dispose();
        joystickDevice = null!;
    }
}