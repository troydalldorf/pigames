using System.Device.Gpio;
using System.Device.I2c;
using Core.Inputs.Seesaw;

namespace Core.Inputs;

public class PlayerConsole : IDisposable, IPlayerConsole
{
    private const int BusId = 1;
    private ConsoleOptions options;
    private I2cDevice joystickDevice;
    private Attiny8X7SeeSaw joystickSeesaw;
    private I2cDevice buttonsDevice;
    private Attiny8X7SeeSaw buttonsSeesaw;
    private const ulong Pin18Right = 1 << 18;
    private const ulong Pin19Left = 1 << 19;
    private const ulong Pin20down = 1 << 20;
    private const ulong Pin2Up = 1 << 2;
    private const ulong AllJoystickPins = Pin18Right | Pin19Left | Pin20down | Pin2Up;
    private const ulong Pin18Red = 1 << 18;
    private const ulong Pin19Yellow = 1 << 19;
    private const ulong Pin20Green = 1 << 20;
    private const ulong Pin2Blue = 1 << 2;
    private const ulong AllButtonPins = Pin18Red | Pin19Yellow | Pin20Green | Pin2Blue;
    private const ulong Pin12RedLed = 1 << 12;
    private const ulong Pin13YellowLed = 1 << 13;
    private const ulong Pin0GreenLed = 1 << 0;
    private const ulong Pin1BlueLed = 1 << 1;
    private const ulong AllLeds = Pin12RedLed | Pin13YellowLed | Pin0GreenLed | Pin1BlueLed;

    public PlayerConsole(int joystickAddress, int buttonsAddress, ConsoleOptions? options = null)
    {
        this.options = options ?? new ConsoleOptions(true);
        
        joystickDevice = I2cDevice.Create(new I2cConnectionSettings(BusId, joystickAddress));
        joystickSeesaw = new Attiny8X7SeeSaw(joystickDevice);
        joystickSeesaw.SetGpioPinModeBulk(AllJoystickPins, PinMode.InputPullUp);
        
        buttonsDevice = I2cDevice.Create(new I2cConnectionSettings(BusId, buttonsAddress));
        buttonsSeesaw = new Attiny8X7SeeSaw(buttonsDevice);
        buttonsSeesaw.SetGpioPinModeBulk(AllButtonPins, PinMode.InputPullUp);
        buttonsSeesaw.SetGpioPinModeBulk(AllLeds, PinMode.Output);
    }

    public virtual JoystickDirection ReadJoystick()
    {
        var data = joystickSeesaw.ReadGpioDigitalBulk(AllJoystickPins);
        var result = JoystickDirection.None;
        if ((data & Pin18Right) == 0) result |= JoystickDirection.Right;
        if ((data & Pin19Left) == 0) result |= JoystickDirection.Left;
        if ((data & Pin20down) == 0) result |= JoystickDirection.Down;
        if ((data & Pin2Up) == 0) result |= JoystickDirection.Up;
        return result;
    }
    
    public Buttons ReadButtons()
    {
        var data = buttonsSeesaw.ReadGpioDigitalBulk(AllButtonPins);
        var result = Buttons.None;
        if ((data & Pin18Red) == 0) result |= Buttons.Red;
        if ((data & Pin19Yellow) == 0) result |= Buttons.Yellow;
        if ((data & Pin20Green) == 0) result |= Buttons.Green;
        if ((data & Pin2Blue) == 0) result |= Buttons.Blue;

        if (this.options.LightButtonOnPress)
        {
            LightButtons(result.IsRedPushed(), result.IsGreenPushed(), result.IsBluePushed(), result.IsYellowPushed());
        }
        
        return result;
    }

    public void LightButtons(bool red, bool green, bool blue, bool yellow)
    {
        buttonsSeesaw.WriteGpioDigital(12, red);
        buttonsSeesaw.WriteGpioDigital(13, yellow);
        buttonsSeesaw.WriteGpioDigital(1, blue);
        buttonsSeesaw.WriteGpioDigital(0, green);
    }

    public void Dispose()
    {
        joystickSeesaw.Dispose();
        joystickSeesaw = null!;
        joystickDevice.Dispose();
        joystickDevice = null!;
    }
}