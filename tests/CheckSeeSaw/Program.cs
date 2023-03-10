// See https://aka.ms/new-console-template for more information

using System.Device.Gpio;
using System.Device.I2c;
using CheckSeeSaw;

Console.WriteLine("Hello, World!");

using var i2CDevice = I2cDevice.Create(new I2cConnectionSettings(1,0x3a));
using var seesaw = new MySeesaw(i2CDevice);
ulong pin18 = 1 << 18;
ulong pin19 = 1 << 19;
ulong pin20 = 1 << 20;
ulong pin2 = 1 << 2;
var pins = pin18 | pin19 | pin20 | pin2;
seesaw.SetGpioPinModeBulk(pins, PinMode.InputPullUp);
Console.WriteLine($"version: {seesaw.Version}");
while (true)
{
    var data = seesaw.ReadGpioDigitalBulk(0);
    var bitStrM = Convert.ToString((long)data, 2).PadLeft(64, '0');
    var bitStrS = Convert.ToString((long)data, 2).PadLeft(64, '0');
    Console.WriteLine($"my: {bitStrM} | simple: {bitStrS}");
    System.Threading.Thread.Sleep(50);
}