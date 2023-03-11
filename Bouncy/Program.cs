using Bouncy;
using Core.Display;
using Core.Inputs;

Console.WriteLine("Starting...");
var player1 = new Player(0x3a);
var display = new LedDisplay();
var ball = new Physics { X = 32, Y = 32, VelocityY = 0, Gravity = 1, Elasticity = 2, Friction = 1 };
while (true)
{
    display.Clear();
    ball.Y += ball.VelocityY;
    ball.VelocityY += ball.Gravity;
    if (ball.Y > 64)
    {
        ball.Y = 63;
        ball.VelocityY = -ball.VelocityY / ball.Elasticity - ball.Friction;
    }
    display.DrawCircle(ball.X, ball.Y, 1, new Color(155, 75, 155));
    display.Update();
}