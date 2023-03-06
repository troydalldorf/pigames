// See https://aka.ms/new-console-template for more information

using System.Device.Gpio;

Console.WriteLine("Hello, World!");
var buttonPin = 27;
using GpioController controller = new();
controller.OpenPin(buttonPin, PinMode.InputPullUp);

while (true)
{
    if (controller.Read(buttonPin) == PinValue.Low)
    {
        Console.WriteLine("Down");
    }
    else
    {
        Console.WriteLine("None");
    }
} 