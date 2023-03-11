using Core.Display;
using Core.Inputs;

Console.WriteLine("Starting...");
var player1 = new Player(0x3a);
var display = new LedDisplay();

while (true)
{
    display.Clear();
    display.SetPixel(32, 32, new Color(155, 0, 0));
    var js = player1.ReadJoystick();
}