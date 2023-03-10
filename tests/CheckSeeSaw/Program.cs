// See https://aka.ms/new-console-template for more information

using System.Device.I2c;
using CheckSeeSaw;

Console.WriteLine("Hello, World!");

using var i2CDevice = I2cDevice.Create(new I2cConnectionSettings(1,0x3a));
using var seeSaw = new MySeesaw(i2CDevice, 10);
Console.WriteLine($"version: {seeSaw.Version}");
while (true)
{
    Console.WriteLine($"{seeSaw.ReadGpioDigital(18)}, {seeSaw.ReadGpioDigital(19)}, {seeSaw.ReadGpioDigital(20)}, {seeSaw.ReadGpioDigital(2)}");
    System.Threading.Thread.Sleep(50);
}