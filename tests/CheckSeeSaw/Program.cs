// See https://aka.ms/new-console-template for more information

using System.Device.I2c;
using CheckSeeSaw;
using Iot.Device.Seesaw;

Console.WriteLine("Hello, World!");

using var i2CDevice = I2cDevice.Create(new I2cConnectionSettings(1,0x3a));
using var seeSaw = new MySeesaw(i2CDevice);
Console.WriteLine($"version: {seeSaw.Version}");
while (true)
{
    Console.WriteLine($"{seeSaw.ReadGpioDigital(1)}, {seeSaw.ReadGpioDigital(2)}, {seeSaw.ReadGpioDigital(3)}, {seeSaw.ReadGpioDigital(4)}");
    System.Threading.Thread.Sleep(50);
}