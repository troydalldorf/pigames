using Core.Display;
using Core.Inputs;

Console.WriteLine("Starting...");
var player1 = new Player(0x3a);
var display = new LedDisplay();
var x = 32;
var y = 32;
while (true)
{
    display.Clear();
    var js = player1.ReadJoystick();
    if (js.IsUp())
    {
        y = y + 1;
    }
    if (js.IsDown())
    {
        y = y - 1;
    }
    if (js.IsLeft())
    {
        x = x - 1;
    }
    if (js.IsRight())
    {
        x = x + 1;
    }
    display.SetPixel(32, 32, new Color(155, 75, 155));
    display.Update();
}