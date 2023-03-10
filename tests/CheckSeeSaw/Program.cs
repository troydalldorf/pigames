// See https://aka.ms/new-console-template for more information

using System.Device.Gpio;
using System.Device.I2c;
using CheckSeeSaw;
using Iot.Device.Seesaw;

Console.WriteLine("Hello, World!");

using var i2CDevice = I2cDevice.Create(new I2cConnectionSettings(1,0x3a));
using var seeSaw = new MySeesaw(i2CDevice);
seeSaw.SetGpioPinMode(18, PinMode.Input);
seeSaw.SetGpioPinMode(19, PinMode.Input);
seeSaw.SetGpioPinMode(20, PinMode.Input);
seeSaw.SetGpioPinMode(2, PinMode.Input);
Console.WriteLine($"version: {seeSaw.Version}");
while (true)
{
    Console.WriteLine($"{seeSaw.ReadGpioDigital(18)}, {seeSaw.ReadGpioDigital(19)}, {seeSaw.ReadGpioDigital(20)}, {seeSaw.ReadGpioDigital(2)}");
    System.Threading.Thread.Sleep(50);
}