using Core.Display;
using Core.Inputs;

namespace Breakout;

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Threading;

class BreakoutGame
{
    private const int Width = 64;
    private const int Height = 64;
    private const int PaddleWidth = 8;
    private const int PaddleHeight = 2;
    private const int BallSize = 2;
    private const int BrickWidth = 6;
    private const int BrickHeight = 2;

    private LedDisplay display;
    private PlayerConsole playerConsole;

    private int paddleX;
    private Rectangle ball;
    private int ballDX = 1;
    private int ballDY = -1;
    private List<Rectangle> bricks;

    public BreakoutGame(LedDisplay display, PlayerConsole playerConsole)
    {
        this.display = display;
        this.playerConsole = playerConsole;
    }

    public void Run()
    {
        Initialize();

        while (true)
        {
            Update();
            Draw();
            Thread.Sleep(50);
        }
    }

    private void Initialize()
    {
        paddleX = Width / 2 - PaddleWidth / 2;
        ball = new Rectangle(Width / 2 - BallSize / 2, Height / 2 - BallSize / 2, BallSize, BallSize);

        bricks = new List<Rectangle>();

        for (int y = 0; y < 5; y++)
        {
            for (int x = 0; x < 10; x++)
            {
                bricks.Add(new Rectangle(x * (BrickWidth + 1), y * (BrickHeight + 1), BrickWidth, BrickHeight));
            }
        }
    }

    private void Update()
    {
        // Update paddle
        var stick = playerConsole.ReadJoystick();
        if (stick.IsLeft()) paddleX--;
        if (stick.IsRight()) paddleX++;
        paddleX = Math.Clamp(paddleX, 0, Width - PaddleWidth);

        // Update ball
        ball.X += ballDX;
        ball.Y += ballDY;

        // Collision with walls
        if (ball.Left < 0 || ball.Right > Width)
        {
            ballDX = -ballDX;
        }

        if (ball.Top < 0)
        {
            ballDY = -ballDY;
        }

        if (ball.Bottom > Height)
        {
            // Game over
            Initialize();
            return;
        }

        // Collision with paddle
        Rectangle paddle = new Rectangle(paddleX, Height - PaddleHeight, PaddleWidth, PaddleHeight);
        if (ball.IntersectsWith(paddle))
        {
            ballDY = -ballDY;
        }

        // Collision with bricks
        for (int i = bricks.Count - 1; i >= 0; i--)
        {
            if (ball.IntersectsWith(bricks[i]))
            {
                bricks.RemoveAt(i);
                ballDY = -ballDY;
                break;
            }
        }

        // Check for victory
        if (bricks.Count == 0)
        {
            // Win
            Initialize();
            return;
        }
    }

    private void Draw()
    {
        display.Clear();

        // Draw paddle
        display.DrawRectangle(paddleX, Height - PaddleHeight, PaddleWidth, PaddleHeight, Color.White);

        // Draw ball
        display.DrawRectangle(ball.X, ball.Y, BallSize, BallSize, Color.Red);

        // Draw bricks
        Color[] brickColors = { Color.Magenta, Color.Blue, Color.Green, Color.Yellow, Color.Orange };
        int row = 0;
        for (int i = 0; i < bricks.Count; i++)
        {
            if (i % 10 == 0 && i > 0)
            {
                row++;
            }
            display.DrawRectangle(bricks[i].X, bricks[i].Y, bricks[i].Width, bricks[i].Height, brickColors[row % brickColors.Length]);
        }

        display.Update();
    }
}