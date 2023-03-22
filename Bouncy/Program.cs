using System.Drawing;
using Core;
using Core.Display;
using Core.Inputs;

var display = new LedDisplay();
var bx=16
var by=4
int x = 45;
int y = 5;
int dx = 1;
int dy = 1;
int px = 16;
int py = 60;
var p1Console = new PlayerConsole(0x3a, 0x42);
var p2Console = new PlayerConsole(0x44, 0x43);

while (true)
{
    //p2 joystick
    var stick = p2Console.ReadJoystick();
    if (stick.IsLeft()) bx--;
    if (stick.IsRight()) bx++;
    if (stick.IsUp()) by--;
    if (stick.IsDown()) by++;
    //p1 joystick
    var stick = p1Console.ReadJoystick();
    if (stick.IsLeft()) px--;
    if (stick.IsRight()) px++;
    if (stick.IsUp()) py--;
    if (stick.IsDown()) py++;
    // ball code
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
        // dy was 1
        dy = 0;
    }
    if(y>63)
    {
        // dy was -1
        dy = 0;
    }
    
    display.Clear();
    display.DrawRectangle(x, y, 2, 2, Color.FromArgb(175, 100, 255));
    display.DrawRectangle(px, py, 20, 4, Color.FromArgb(0, 255, 255));
    display.DrawRectangle(bx, by, 20, -4, Color.FromArgb(255, 255, 0));
    display.Update();
    
    
}
      