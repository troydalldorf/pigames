using System.Drawing;
using Core.Display;

var display = new LedDisplay();
int x = 45;
int y = 5;
int dx = 1;
int dy = 1;
while (true)
{
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
    display.DrawRectangle(16, 60, 20, 4, Color.FromArgb(155, 155, 155));
    display.Update();
}
      