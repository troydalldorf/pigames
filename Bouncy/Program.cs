using System.Drawing;
using Core.Display;

var display = new LedDisplay();
int x = 20;
int y = 20;
int dx = 1;
int dy = 1;
while (true)
{
    display.Clear();
    x = x + dx;
    y = y + dy;
    display.DrawCircle(x, x, 1, Color.FromArgb(155, 75, 155));
    display.Update();
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
}