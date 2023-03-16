// See https://aka.ms/new-console-template for more information

using System.Device.Gpio;
using System.Device.I2c;
using CheckSeeSaw;

Console.WriteLine("Hello, World!");

using var p1Joystick = I2cDevice.Create(new I2cConnectionSettings(1,0x3a));
using var p1Buttons = I2cDevice.Create(new I2cConnectionSettings(1,0x42));
using var ssP1Joystick = new MySeesaw(p1Joystick);
using var ssP1Buttons = new MySeesaw(p1Buttons);
ulong pin18 = 1 << 18;
ulong pin19 = 1 << 19;
ulong pin20 = 1 << 20;
ulong pin2 = 1 << 2;
var pins = pin18 | pin19 | pin20 | pin2;
ssP1Joystick.SetGpioPinModeBulk(pins, PinMode.InputPullUp);
ssP1Buttons.SetGpioPinMode(12, PinMode.Output);
while (true)
{
    // P1 Joystick
    var dataP1Joystick = ssP1Joystick.ReadGpioDigitalBulk(0);
    var right = (dataP1Joystick & pin18) == 0;
    var left = (dataP1Joystick & pin19) == 0;
    var down = (dataP1Joystick & pin20) == 0;
    var up = (dataP1Joystick & pin2) == 0;
    var dataP1Buttons = ssP1Joystick.ReadGpioDigitalBulk(0);
    var red = (dataP1Buttons & pin18) == 0;
    var green = (dataP1Buttons & pin19) == 0;
    var blue = (dataP1Buttons & pin20) == 0;
    Console.WriteLine($"P1 => up: {up}, dn: {down}, lt: {left}, rt: {right} | red:{red}, green: {green}, blue: {blue}");
    //ssP1Buttons.WriteGpioDigital(12, left | right | down | up);
}