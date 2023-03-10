// See https://aka.ms/new-console-template for more information

using System.Device.Gpio;
using System.Device.I2c;
using CheckSeeSaw;

Console.WriteLine("Hello, World!");

using var i2CDevice = I2cDevice.Create(new I2cConnectionSettings(1,0x3a));
using var seesaw = new MySeesaw(i2CDevice);
//seesaw.TestPinModeBulk(4, 0, 1 << 18, PinMode.InputPullDown);
Console.WriteLine($"version: {seesaw.Version}");
while (true)
{
    Console.WriteLine($"{seesaw.ReadGpioDigitalBulk(0):X16} | simple: {seesaw.TestDigitalReadBulk(25):X16}");
    System.Threading.Thread.Sleep(50);
}