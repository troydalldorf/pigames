using System.Diagnostics;
using Core.Display;
using Core.Display.Sprites;
using Core.Inputs;

var image = new SpriteImage("graphics.png");
var alien1a = image.GetSprite(0, 0, 8, 8);
var alien1b = image.GetSprite(9, 0, 8, 8);
var alien1c = image.GetSprite(18, 0, 8, 8);
var alien1 = new SpriteAnimation(8, 8, alien1a, alien1b, alien1c);
var display = new LedDisplay();
var player = new Player(0x3a, 0x42);
var stopwatch = new Stopwatch();
while (true)
{
    display.Clear();
    alien1.Frame = ((int)stopwatch.Elapsed.TotalMilliseconds / 250) % 2; 
    display.DrawSprite(20, 20, alien1);
    display.Update();
}