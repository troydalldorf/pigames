// See https://aka.ms/new-console-template for more information

using System;
using System.Device.Gpio;

Console.WriteLine("Hello, World!");

var buttonPin = 27;
using GpioController controller = new();
controller.OpenPin(buttonPin, PinMode.InputPullUp);

while (true)
{
    Console.WriteLine(controller.Read(buttonPin) == PinValue.Low ? "Down" : "None");
} 