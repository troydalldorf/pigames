using System.Drawing;
using Core;
using Core.Display;
using Core.Inputs;

var display = new LedDisplay();
var bx = 16;
var by = 4;
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
    if (x == px && y == py )
    {
        x = 32;
        y = 32;
    }
    if (x == bx && y == by )
    {
        x = 32;
        y = 32;
    }
   
    
    //p2 joystick
    var stick2 = p2Console.ReadJoystick();
    if (stick2.IsLeft()) bx++;
    if (stick2.IsRight()) bx--;
    if (stick2.IsUp()) by++;
    if (stick2.IsDown()) by--;
    //p1 joystick
    var stick1 = p1Console.ReadJoystick();
    if (stick1.IsLeft()) px--;
    if (stick1.IsRight()) px++;
    if (stick1.IsUp()) py--;
    if (stick1.IsDown()) py++;
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
    display.DrawRectangle(bx, by, 20, -3, Color.FromArgb(255, 255, 0));
    display.Update();
    
    
}
      