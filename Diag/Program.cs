// See https://aka.ms/new-console-template for more information

using Core.Inputs;

var player1 = new Player1Console();
var player2 = new Player2Console();

Console.WriteLine("Input Diagnostics");
Console.WriteLine("P1 and P2 console activity will be reported here.");
Console.WriteLine("Press a button on the keyboard to exit.");
while (Console.Read() == -1)
{
    var buttons = player1.ReadButtons();
    if (buttons != 0)
        Console.WriteLine($"P1 Buttons: {buttons.ToString()}");
    var stick = player1.ReadJoystick();
    if (stick != 0)
        Console.WriteLine($"P1 Stick: {stick}");
    buttons = player2.ReadButtons();
    if (buttons != 0)
        Console.WriteLine($"P2 Buttons: {buttons.ToString()}");
    stick = player2.ReadJoystick();
    if (stick != 0)
        Console.WriteLine($"P2 Stick: {stick}");
}