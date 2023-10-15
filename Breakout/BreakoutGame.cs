using Breakout.Bits;
using Core;
using Core.Effects;
using System.Drawing;
using Core.Display;

namespace Breakout;

public class BreakoutGame : IPlayableGameElement
{
    private const int Width = 64;
    private const int Height = 64;
    private int paddleWidth = 8;
    private const int PaddleHeight = 2;
    private const int BallSize = 2;
    private const float BallSpeed = 1.5f;

    readonly Color[] brickColors = { Color.Magenta, Color.Blue, Color.Green, Color.Yellow, Color.Orange };

    private int paddleX;
    private float ballX;
    private float ballY;
    private Rectangle Ball => new((int)ballX, (int)ballY, BallSize, BallSize);
    private float ballDx = BallSpeed;
    private float ballDy = -BallSpeed;
    private readonly List<Brick> bricks = new();
    private readonly List<PixelBomb> pixelBombs = new();
    private bool isDone;
    private readonly List<PowerUp> powerUps = new();
    private bool isStickyPaddle = false;
    private readonly Random random = new();
    const float PowerUpChance = 0.2f;

    public BreakoutGame()
    {
        Initialize();
    }

    private void Initialize()
    {
        paddleX = Width / 2 - paddleWidth / 2;
        ballX = Width / 2 - BallSize / 2;
        ballY = Height / 2 - BallSize / 2;

        pixelBombs.Clear();

        for (var y = 0; y < 5; y++)
        {
            for (var x = 0; x < 10; x++)
            {
                bricks.Add(new Brick(x * (Brick.BrickWidth + 1), y * (Brick.BrickHeight + 1), brickColors[y % brickColors.Length]));
            }
        }
    }

    public void HandleInput(IPlayerConsole player1Console)
    {
        var stick = player1Console.ReadJoystick();
        if (stick.IsLeft()) paddleX -= 2;
        if (stick.IsRight()) paddleX += 2;
        paddleX = Math.Clamp(paddleX, 0, Width - paddleWidth);
    }

    public void Update()
    {
        foreach (var bomb in pixelBombs.ToArray())
        {
            bomb.Update();
            if (bomb.IsExtinguished()) pixelBombs.Remove(bomb);
        }

        // Update ball
        ballX += ballDx;
        ballY += ballDy;

        // Collision with walls
        if (Ball.Left < 0 || Ball.Right > Width)
        {
            ballDx = -ballDx;
        }

        if (Ball.Top < 0)
        {
            ballDy = -ballDy;
        }

        // Game over
        if (Ball.Bottom > Height)
        {
            foreach (var brick in bricks)
            {
                pixelBombs.Add(new PixelBomb(brick.X + 2, brick.Y + 2, Brick.BrickWidth * Brick.BrickHeight, brick.Color, 3));
            }

            bricks.Clear();

            isDone = true;
            return;
        }

        // Collision with paddle
        var paddle = new Rectangle(paddleX, Height - PaddleHeight, paddleWidth, PaddleHeight);
        if (Ball.IntersectsWith(paddle))
        {
            float paddleCenter = paddleX + paddleWidth / 2.0f;
            float ballCenter = ballX + BallSize / 2.0f;
            float relativeIntersectX = (paddleCenter - ballCenter) / (paddleWidth / 2.0f);
            const float maxBounceAngle = 50.0f;
            float bounceAngle = relativeIntersectX * maxBounceAngle;
            float radians = (float)(bounceAngle * (Math.PI / 180.0));

            ballDx = (float)Math.Sin(radians) * BallSpeed;
            ballDy = -(float)Math.Cos(radians) * BallSpeed;
        }

        // Collision with bricks
        for (var i = bricks.Count - 1; i >= 0; i--)
        {
            if (!bricks[i].IntersectsWith(Ball)) continue;
            // Power ups
            if (random.NextDouble() < PowerUpChance)
            {
                var powerUpType = (PowerUpType)random.Next(Enum.GetValues(typeof(PowerUpType)).Length);
                var powerUpBounds = new Rectangle(bricks[i].X + Brick.BrickWidth / 2, bricks[i].Y + Brick.BrickHeight / 2, 3, 3);
                var powerUpColor = Color.Cyan;
                var powerUp = new PowerUp(powerUpType, powerUpBounds, powerUpColor);
                powerUps.Add(powerUp);
            }
            pixelBombs.Add(new PixelBomb(bricks[i].X + 2, bricks[i].Y + 2, Brick.BrickWidth * Brick.BrickHeight, bricks[i].Color));
            bricks.RemoveAt(i);
            ballDy = -ballDy;
            break;
        }

        // Check for victory
        if (bricks.Count == 0)
        {
            isDone = true;
        }
        
        // Update power-ups
        foreach (var powerUp in powerUps.ToArray())
        {
            if (powerUp.IsActive)
            {
                powerUp.Bounds = new Rectangle(powerUp.Bounds.X, powerUp.Bounds.Y + 1, powerUp.Bounds.Width, powerUp.Bounds.Height);

                if (powerUp.Bounds.IntersectsWith(paddle))
                {
                    ActivatePowerUp(powerUp);
                    powerUp.IsActive = false;
                }
                else if (powerUp.Bounds.Bottom > Height)
                {
                    powerUp.IsActive = false;
                }
            }
            else
            {
                powerUps.Remove(powerUp);
            }
        }
    }
    
    private void ActivatePowerUp(PowerUp powerUp)
    {
        switch (powerUp.Type)
        {
            case PowerUpType.PaddleSizeIncrease:
                paddleWidth = Math.Min(paddleWidth + 4, Width);
                break;
            case PowerUpType.MultiBall:
                // Logic for multiple balls
                break;
            case PowerUpType.StickyPaddle:
                isStickyPaddle = true;
                break;
        }
    }

    public void Draw(IDisplay display)
    {
        display.DrawRectangle(paddleX, Height - PaddleHeight, paddleWidth, PaddleHeight, Color.White);
        display.DrawRectangle(Ball.X, Ball.Y, BallSize, BallSize, Color.Red);

        // Draw bricks
        foreach (var brick in bricks)
            display.DrawRectangle(brick.X, brick.Y, Brick.BrickWidth, Brick.BrickHeight, brick.Color);
        
        // Draw power-ups
        foreach (var powerUp in powerUps.Where(powerUp => powerUp.IsActive))
            display.DrawRectangle(powerUp.Bounds.X, powerUp.Bounds.Y, powerUp.Bounds.Width, powerUp.Bounds.Height, powerUp.Color);

        foreach (var bomb in pixelBombs)
            bomb.Draw(display);
    }

    public GameOverState State => isDone ? GameOverState.EndOfGame : GameOverState.None;
}