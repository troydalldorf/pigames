using System.Drawing;
using Core;
using Core.Display;
using Core.Inputs;

Console.WriteLine("Starting...");
var player1 = new PlayerConsole(0x3a, 0x42);
var display = new LedDisplay();
var x = 32;
var y = 32;
var rnd = new Random();
var color = RandomColor();
while (true)
{
    display.Clear();
    var button = player1.ReadButtons();
    if (button > 0) color = RandomColor();
    var js = player1.ReadJoystick();
    if (js.IsUp())
    {
        y = y - 1;
    }
    if (js.IsDown())
    {
        y = y + 1;
    }
    if (js.IsLeft())
    {
        x = x - 1;
    }
    if (js.IsRight())
    {
        x = x + 1;
    }
    if( x>63)
    {
        x = 0;
    }
    if( y>63)
    {
        y = 0;
    }
    if( y<0)
    {
        y = 63;
    }
    if( x<0)
    {
        x = 63;
    }
    display.SetPixel(x, y, color);
    display.Update();
}

Color RandomColor()
{
    return Color.FromArgb(rnd.Next(1, 255), rnd.Next(1, 255), rnd.Next(1, 255));
}