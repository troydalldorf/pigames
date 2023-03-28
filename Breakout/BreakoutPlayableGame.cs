using Core;
using Core.Effects;

namespace Breakout;

using System;
using System.Collections.Generic;
using System.Drawing;

public class BreakoutPlayableGame : IPlayableGameElement
{
    private const int Width = 64;
    private const int Height = 64;
    private const int PaddleWidth = 8;
    private const int PaddleHeight = 2;
    private const int BallSize = 2;
    private const int BrickWidth = 6;
    private const int BrickHeight = 2;

    readonly Color[] brickColors = { Color.Magenta, Color.Blue, Color.Green, Color.Yellow, Color.Orange };

    private int paddleX;
    private Rectangle ball;
    private int ballDx = 1;
    private int ballDy = -1;
    private List<Brick> bricks = new();
    private readonly List<PixelBomb> pixelBombs = new();
    private bool isDone;

    public BreakoutPlayableGame()
    {
        Initialize();
    }

    private void Initialize()
    {
        paddleX = Width / 2 - PaddleWidth / 2;
        ball = new Rectangle(Width / 2 - BallSize / 2, Height / 2 - BallSize / 2, BallSize, BallSize);

        pixelBombs.Clear();
        
        for (var y = 0; y < 5; y++)
        {
            for (var x = 0; x < 10; x++)
            {
                bricks.Add(new Brick(x * (BrickWidth + 1), y * (BrickHeight + 1), brickColors[y % brickColors.Length]));
            }
        }
    }

    public void HandleInput(IPlayerConsole player1Console)
    {
        var stick = player1Console.ReadJoystick();
        if (stick.IsLeft()) paddleX -= 2;
        if (stick.IsRight()) paddleX += 2;
        paddleX = Math.Clamp(paddleX, 0, Width - PaddleWidth);
    }

    public void Update()
    {
        foreach (var bomb in pixelBombs.ToArray())
        {
            bomb.Update();
            if (bomb.IsExtinguished()) pixelBombs.Remove(bomb);
        }

        // Update ball
        ball.X += ballDx;
        ball.Y += ballDy;

        // Collision with walls
        if (ball.Left < 0 || ball.Right > Width)
        {
            ballDx = -ballDx;
        }

        if (ball.Top < 0)
        {
            ballDy = -ballDy;
        }

        // Game over
        if (ball.Bottom > Height)
        {
            foreach (var brick in bricks)
            {
                pixelBombs.Add(new PixelBomb(brick.X+2, brick.Y+2, BrickWidth*BrickHeight, brick.Color, 3));
            }

            bricks.Clear();

            isDone = true;
            return;
        }

        // Collision with paddle
        var paddle = new Rectangle(paddleX, Height - PaddleHeight, PaddleWidth, PaddleHeight);
        if (ball.IntersectsWith(paddle))
        {
            ballDy = -ballDy;
        }

        // Collision with bricks
        var row = 0;
        for (var i = bricks.Count - 1; i >= 0; i--)
        {
            if (i % 10 == 0 && i > 0)
            {
                row++;
            }
            if (!bricks[i].IntersectsWith(ball)) continue;
            pixelBombs.Add(new PixelBomb(bricks[i].X+2, bricks[i].Y+2, BrickWidth*BrickHeight, bricks[i].Color));
            bricks.RemoveAt(i);
            ballDy = -ballDy;
            break;
        }

        // Check for victory
        if (bricks.Count == 0)
        {
            isDone = true;
        }
    }

    public void Draw(IDisplay display)
    {
        display.DrawRectangle(paddleX, Height - PaddleHeight, PaddleWidth, PaddleHeight, Color.White);
        display.DrawRectangle(ball.X, ball.Y, BallSize, BallSize, Color.Red);

        // Draw bricks
        var row = 0;
        for (var i = 0; i < bricks.Count; i++)
        {
            if (i % 10 == 0 && i > 0)
            {
                row++;
            }
            display.DrawRectangle(bricks[i].X, bricks[i].Y, BrickWidth, BrickHeight, bricks[i].Color);
        }
        
        foreach (var bomb in pixelBombs)
        {
            bomb.Draw(display);
        }
    }

    public GameOverState State => isDone ? GameOverState.EndOfGame : GameOverState.None;

    private record Brick(int X, int Y, Color Color)
    {
        public bool IntersectsWith(Rectangle r)
        {
            var brickRight = X + BrickWidth;
            var brickBottom = Y + BrickHeight;
            return X < r.Right && brickRight > r.Left && Y < r.Bottom && brickBottom > r.Top;
        }
    }
}