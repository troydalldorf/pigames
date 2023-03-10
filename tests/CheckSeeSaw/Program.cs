// See https://aka.ms/new-console-template for more information

using System.Device.Gpio;
using System.Device.I2c;
using CheckSeeSaw;

Console.WriteLine("Hello, World!");

using var i2CDevice = I2cDevice.Create(new I2cConnectionSettings(1,0x3a));
using var seeSaw = new MySeesaw(i2CDevice);
seeSaw.TestPinModeBulk(4, 0, 1 << 18, PinMode.InputPullDown);
Console.WriteLine($"version: {seeSaw.Version}");
while (true)
{
    Console.WriteLine($"{seeSaw.TestDigitalReadBulk():X16}");
    System.Threading.Thread.Sleep(50);
}