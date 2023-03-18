// See https://aka.ms/new-console-template for more information

using Core.Inputs;

Console.WriteLine("Hello, World!");

using var p1Console = new PlayerConsole(0x3a, 0x42);
using var p2Console = new PlayerConsole(0x44, 0x43);
while (true)
{
    var p1Stick = p1Console.ReadJoystick();
    var p1Buttons = p1Console.ReadButtons();
    var p2Stick = p2Console.ReadJoystick();
    var p2Buttons = p2Console.ReadButtons();
    Console.WriteLine($"P1 => stick: {p1Stick.ToString()} | buttons: {p1Buttons.ToString()} || P2 => stick: {p2Stick.ToString()} | buttons: {p2Buttons.ToString()}");
    p1Console.LightButtons(p1Buttons.IsRedPushed(), p1Buttons.IsGreenPushed(), p1Buttons.IsBluePushed(), false);
    p2Console.LightButtons(p2Buttons.IsRedPushed(), p2Buttons.IsGreenPushed(), p2Buttons.IsBluePushed(), false);
}