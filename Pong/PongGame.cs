using Core.Inputs;

namespace Pong;

using System;
using System.Drawing;
using System.Threading;
using Core.Display;

internal class PongGame
{
    private const int Width = 64;
    private const int Height = 64;
    private const int PaddleWidth = 2;
    private const int PaddleHeight = 8;
    private const int BallSize = 2;

    private LedDisplay display;
    private PlayerConsole player1Console;
    private PlayerConsole player2Console;

    private Rectangle player1Paddle;
    private Rectangle player2Paddle;
    private Point ballPosition;
    private Point ballVelocity;
    private Random random;

    public PongGame(LedDisplay display, PlayerConsole player1Console, PlayerConsole player2Console)
    {
        this.display = display;
        this.player1Console = player1Console;
        this.player2Console = player2Console;

        player1Paddle = new Rectangle(1, Height / 2 - PaddleHeight / 2, PaddleWidth, PaddleHeight);
        player2Paddle = new Rectangle(Width - 1 - PaddleWidth, Height / 2 - PaddleHeight / 2, PaddleWidth, PaddleHeight);
        ballPosition = new Point(Width / 2, Height / 2);
        random = new Random();
        ResetBall();
    }

    public void Run()
    {
        while (true)
        {
            HandleInput();
            Update();
            Draw();
            Thread.Sleep(50);
        }
    }

    private void HandleInput()
    {
        var stick1 = player1Console.ReadJoystick();
        var stick2 = player2Console.ReadJoystick();

        if (stick1.IsUp() && player1Paddle.Top > 0)
            player1Paddle.Y -= 2;
        if (stick1.IsDown() && player1Paddle.Bottom < Height)
            player1Paddle.Y += 2;

        if (stick2.IsUp() && player2Paddle.Top > 0)
            player2Paddle.Y -= 2;
        if (stick2.IsDown() && player2Paddle.Bottom < Height)
            player2Paddle.Y += 2;
    }

    private void Update()
    {
        ballPosition.X += ballVelocity.X;
        ballPosition.Y += ballVelocity.Y;

        // Ball collision with top and bottom walls
        if (ballPosition.Y <= 0 || ballPosition.Y + BallSize >= Height)
        {
            ballVelocity.Y = -ballVelocity.Y;
        }

        // Ball collision with paddles
        if (ballPosition.X <= player1Paddle.Right && ballPosition.Y + BallSize >= player1Paddle.Top && ballPosition.Y <= player1Paddle.Bottom)
        {
            ballVelocity.X = -ballVelocity.X;
            ballPosition.X = player1Paddle.Right;
        }
        else if (ballPosition.X + BallSize >= player2Paddle.Left && ballPosition.Y + BallSize >= player2Paddle.Top && ballPosition.Y <= player2Paddle.Bottom)
        {
            ballVelocity.X = -ballVelocity.X;
            ballPosition.X = player2Paddle.Left - BallSize;
        }

        // Ball out of bounds (scoring)
        if (ballPosition.X < 0 || ballPosition.X + BallSize > Width)
        {
            ResetBall();
        }
    }

    private void ResetBall()
    {
        ballPosition = new Point(Width / 2, Height / 2);
        var angle = random.Next(30, 150);
        var radians = angle * Math.PI / 180;
        var speed = 2;

        ballVelocity = new Point(
            (int)Math.Round(speed * Math.Cos(radians)) * (random.Next(2) == 0 ? 1 : -1),
            (int)Math.Round(speed * Math.Sin(radians)) * (random.Next(2) == 0 ? 1 : -1)
        );
    }

    private void Draw()
    {
        display.Clear();

        // Draw paddles
        display.DrawRectangle(player1Paddle.X, player1Paddle.Y, PaddleWidth, PaddleHeight, Color.White);
        display.DrawRectangle(player2Paddle.X, player2Paddle.Y, PaddleWidth, PaddleHeight, Color.White);

        // Draw ball
        display.DrawRectangle(ballPosition.X, ballPosition.Y, BallSize, BallSize, Color.White);

        display.Update();
    }
}
