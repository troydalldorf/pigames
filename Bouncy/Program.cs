using System.Drawing;
using Core.Display;
using Core.Inputs;

var display = new LedDisplay();
int x = 45;
int y = 5;
int dx = 1;
int dy = 1;
int px = 16;
var p1Console = new PlayerConsole(0x3a, 0x42);

while (true)
{
    var stick = p1Console.ReadJoystick();
    if (stick.IsLeft()) px--;
    if (stick.IsRight()) px++;
    x = x + dx;
    y = y + dy;
    if(x>63)
    {
        dx = -1;
    }
    if(x<0)
    {
        dx = 1;
    }
    if(y<0)
    {
        dy = 1;
    }
    if(y>63)
    {
        dy = -1;
    }
    
    display.Clear();
    display.DrawRectangle(x, y, 2, 2, Color.FromArgb(0, 255, 0));
    display.DrawRectangle(16, 60, 20, 4, Color.FromArgb(255, 255, 255));
    display.Update();
    
    
}
      