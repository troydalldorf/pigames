using Core.Display;
using Core.Display.Sprites;
using Core.Inputs;

var image = new SpriteImage("graphics.png");
var alien1 = image.GetSpriteAnimation(0, 0, 8, 8, 3, 1);
var alien2 = image.GetSpriteAnimation(0, 9, 8, 8, 3, 1);
var alien3 = image.GetSpriteAnimation(0, 36, 8, 8, 3, 1);
var p1 = image.GetSpriteAnimation(27, 27, 16, 8, 1, 1);
var display = new LedDisplay();
var player = new Player(0x3a, 0x42);
var frame = 0;
while (true)
{
    display.Clear();
    alien1.Frame = alien2.Frame = alien3.Frame = (frame/10) % 2;
    display.DrawSprite(20, 20, alien1);
    display.DrawSprite(28, 20, alien2);
    display.DrawSprite(36, 20, alien3);
    display.DrawSprite(28, 50, p1);
    display.Update();
    frame++;
}