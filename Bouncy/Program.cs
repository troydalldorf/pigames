using System.Drawing;
using Core.Display;

var display = new LedDisplay();
int x = 32;
int y = 15;
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
    display.DrawRectangle( 16, 60, 20, 4, Color.FromArgb(155, 155, 155));
}
      