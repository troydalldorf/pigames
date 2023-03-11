﻿using Core.Display;
using Core.Inputs;

Console.WriteLine("Starting...");
var player1 = new Player(0x3a);
var display = new LedDisplay();
var x = 32;
var y = 32;
while (true)
{
   // display.Clear();
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
    display.SetPixel(x,y, new Color(155, 75, 155));
    display.Update();
}