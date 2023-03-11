using System.Drawing;
using Core.Display;
using Core.Display.Sprites;
using Core.Inputs;

// image
var image = new SpriteImage("graphics.png", new Point(0, 0));
var display = new LedDisplay();

// aliens
var alient1Sprite = image.GetSpriteAnimation(0, 0, 8, 8, 3, 1);
var alient2Sprite = image.GetSpriteAnimation(0, 117, 8, 8, 3, 1);
var alient3Sprite = image.GetSpriteAnimation(0, 36, 8, 8, 3, 1);
var alientVelocityX = 1;
var aliens = new Things();
for (var y = 0; y < 3; y++)
{
    for (var x = 0; x < 6; x++)
    {
        var alien = new Thing("alien", y switch { 0 => alient1Sprite, 1 => alient2Sprite, _ => alient3Sprite }, x * 10, y * 10);
        aliens.Add(alien);
    }
}

// player1
var player1Sprite = image.GetSpriteAnimation(27, 27, 16, 8, 1, 1);
var player1 = new Thing("player", player1Sprite, 28, 57);

// phaser
var phaserSprite = image.GetSpriteAnimation(27, 0, 8, 8, 4, 1);
var p1Console = new PlayerConsole(0x3a, 0x42);
var phasers = new Things();
var frame = 0;
var lastPhaser = frame;
while (true)
{
    // animation
    phasers.SetAllFrameNo((frame / 4) % 4);
    aliens.SetAllFrameNo((frame/10) % 2);
    
    // motion
    var stick = p1Console.ReadJoystick();
    if (stick.IsLeft()) player1.X--;
    if (stick.IsRight()) player1.X++;
    var buttons = p1Console.ReadButtons();
    if (buttons > 0)
    {
        if (frame - lastPhaser < 10) continue;
        phasers.Add(new Thing("phaser", phaserSprite, player1.X+7, 57));
        lastPhaser = frame;
    }
    if (frame/4 % 1 == 0) phasers.MoveAll(0, -1);
    if (frame/10 % 5 == 0) aliens.MoveAll(alientVelocityX, 0);
    if (frame / 10 == 0) aliens.MoveAll(1, 0);
    
    // boundaries
    if (player1.IsBefore(0)) player1.X = 0;
    if (player1.IsAfter(63)) player1.X = 63-player1.Width;
    if (aliens.AreAnyAfter(63) || aliens.AreAnyBefore(0)) alientVelocityX *= -1;
    
    // collisions
    var collisions = phasers.GetCollisions(aliens);
    foreach (var collision in collisions)
    {
        phasers.Remove(collision.A);
        aliens.Remove(collision.Other);
    }
    
    // draw
    display.Clear();
    aliens.DrawAll(display);
    phasers.DrawAll(display);
    player1.Draw(display);
    display.Update();
    frame++;
}