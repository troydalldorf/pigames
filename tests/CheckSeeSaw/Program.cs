// See https://aka.ms/new-console-template for more information

using System.Device.Gpio;
using System.Device.I2c;
using CheckSeeSaw;
using Core.Inputs;

Console.WriteLine("Hello, World!");

using var p1Console = new PlayerConsole(0x3a, 0x42);
while (true)
{
    var p1Stick = p1Console.ReadJoystick();
    var p1Buttons = p1Console.ReadButtons();
    Console.WriteLine($"P1 => stick: {p1Stick.ToString()} | buttons: {p1Buttons.ToString()}");
    //ssP1Buttons.WriteGpioDigital(12, left | right | down | up);
}