using Core;
using Core.Display.Fonts;
using Core.Inputs;

namespace Pong;

using System;
using System.Drawing;
using System.Threading;
using Core.Display;

public class PongGame : I2PGameElement
{
    private const int Width = 64;
    private const int Height = 64;
    private const int PaddleWidth = 8;
    private const int PaddleHeight = 2;
    private const int BallSize = 2;

    private LedFont font;
    private int p1Score = 0;
    private int p2Score = 0;

    private Rectangle player1Paddle;
    private Rectangle player2Paddle;
    private Point ballPosition;
    private Point ballVelocity;
    private Random random;

    public PongGame()
    {
        this.font = new LedFont(LedFontType.FontTomThumb);

        player1Paddle = new Rectangle(Width / 2 - PaddleWidth / 2, Height - 1 - PaddleHeight, PaddleWidth, PaddleHeight);
        player2Paddle = new Rectangle(Width / 2 - PaddleWidth / 2, 0, PaddleWidth, PaddleHeight);
        ballPosition = new Point(Width / 2, Height / 2);
        random = new Random();
        ResetBall();
    }

    public void Run(IDisplay display, IPlayerConsole player1Console, IPlayerConsole player2Console)
    {
        while (true)
        {
            HandleInput(player1Console);
            Handle2PInput(player2Console);
            Update();
            display.Clear();
            Draw(display);
            display.Update();
            Thread.Sleep(50);
        }
    }

    public void HandleInput(IPlayerConsole player1Console)
    {
        var stick1 = player1Console.ReadJoystick();

        if (stick1.IsLeft() && player1Paddle.Left > 0)
            player1Paddle.X -= 2;
        if (stick1.IsRight() && player1Paddle.Right < Width)
            player1Paddle.X += 2;
    }
    
    public void Handle2PInput(IPlayerConsole player2Console)
    {
        var stick2 = player2Console.ReadJoystick();
        if (stick2.IsLeft() && player2Paddle.Right < Width)
            player2Paddle.X += 2;
        if (stick2.IsRight() && player2Paddle.Left < 0)
            player2Paddle.X -= 2;
    }

    public void Update()
    {
        ballPosition.X += ballVelocity.X;
        ballPosition.Y += ballVelocity.Y;

        // Ball collision with side walls
        if (ballPosition.X <= 0 || ballPosition.X + BallSize >= Width)
        {
            ballVelocity.X = -ballVelocity.X;
        }

        // Ball collision with paddles
        if (ballPosition.Y + BallSize >= player1Paddle.Top && ballPosition.X + BallSize >= player1Paddle.Left && ballPosition.X <= player1Paddle.Right)
        {
            ballVelocity.Y = -ballVelocity.Y;
            ballPosition.Y = player1Paddle.Top - BallSize;
        }
        else if (ballPosition.Y <= player2Paddle.Bottom && ballPosition.X + BallSize >= player2Paddle.Left && ballPosition.X <= player2Paddle.Right)
        {
            ballVelocity.Y = -ballVelocity.Y;
            ballPosition.Y = player2Paddle.Bottom;
        }

        // Ball out of bounds (scoring)
        if (ballPosition.Y < 0)
        {
            p2Score++;
            ResetBall();
        }
        else if (ballPosition.Y + BallSize > Height)
        {
            p1Score++;
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

    public void Draw(IDisplay display)
    {
        font.DrawText(display, 1, 28, Color.DimGray, p1Score.ToString(), 0, false);
        font.DrawText(display, 1, 35, Color.DimGray, p2Score.ToString(), 0, false);

        // Draw paddles
        display.DrawRectangle(player1Paddle.X, player1Paddle.Y, PaddleWidth, PaddleHeight, Color.White);
        display.DrawRectangle(player2Paddle.X, player2Paddle.Y, PaddleWidth, PaddleHeight, Color.White);

        // Draw ball
        display.DrawRectangle(ballPosition.X, ballPosition.Y, BallSize, BallSize, Color.White);
    }

    public bool IsDone()
    {
        return false;
    }
}