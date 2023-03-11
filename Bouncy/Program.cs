using System.Drawing;
using Bouncy;
using Core.Display;

Console.WriteLine("Starting...");
var display = new LedDisplay();
var ball = new Physics { X = 32, Y = 1, VelocityY = 0, VelocityX = 1, Gravity = 1, Elasticity = 0.7, Friction = 0.1 };
while (true)
{
    display.Clear();
    ball.Y += ball.VelocityY;
    ball.X += ball.VelocityX;
    ball.VelocityY += ball.Gravity;
    if (ball.Y > 63)
    {
        ball.Y = 63;
        ball.VelocityY = -ball.VelocityY * ball.Elasticity * (1 - ball.Friction);
    }
    if (ball.X > 63)
    {
        ball.VelocityX = -1;
        ball.X = 63;
    }
    if (ball.X < 0)
    {
        ball.VelocityX = +1;
        ball.X = 0;
    }
    display.DrawCircle((int)ball.X, (int)ball.Y, 1,  Color.FromArgb(155, 75, 155));
    display.Update();
}