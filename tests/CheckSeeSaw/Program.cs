// See https://aka.ms/new-console-template for more information

using System.Device.Gpio;
using System.Device.I2c;
using Iot.Device.CharacterLcd;
using Iot.Device.Mcp23xxx;
using Iot.Device.Seesaw;

Console.WriteLine("Hello, World!");

using var i2cDevice = I2cDevice.Create(new I2cConnectionSettings(1,0x3a));
using var seeSaw = new Seesaw(i2cDevice);
while (true)
{
    Console.WriteLine($"{seeSaw.ReadGpioDigital(0)}, {seeSaw.ReadGpioDigital(1)}, {seeSaw.ReadGpioDigital(2)}, {seeSaw.ReadGpioDigital(3)}");
}
