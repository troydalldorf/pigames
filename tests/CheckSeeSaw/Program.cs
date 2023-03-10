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
ulong pin18 = 1 << 0;
ulong pin19 = 1 << 1;
ulong pin20 = 1 << 2;
ulong pin2 = 1 << 3;
ulong pins = pin18 | pin19 | pin20 | pin2;
Console.WriteLine($"version: {seeSaw.Version}");
while (true)
{
    Console.WriteLine($"{seeSaw.TestDigitalReadBulk(pins)}");
    System.Threading.Thread.Sleep(50);
}