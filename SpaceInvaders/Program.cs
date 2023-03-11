using System.Drawing;
using Core.Display;
using Core.Display.Sprites;
using Core.Inputs;

var image = new SpriteImage("graphics.png");
var alien = image.GetSprite(0, 0, 8, 8);
var display = new LedDisplay();
var player = new Player(0x3a, 0x42);
while (true)
{
    display.Clear();
    display.DrawSprite(20, 20, alien);
    display.Update();
}