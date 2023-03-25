using Core;
using Core.Display;
using Core.Inputs;

namespace FlappyBird;

using System;
using System.Drawing;
using System.Threading;

public class FlappyBirdGame : IGameElement
{
    private const int Width = 64;
    private const int Height = 64;
    private const int BirdSize = 4;
    private const int PipeWidth = 8;
    private const int PipeGap = 16;
    private const int NumPipes = 3;
    private const int PipeSpacing = Width / NumPipes;
    private const int Gravity = 1;
    private const int FlapPower = 4;

    private Rectangle bird;
    private int birdVelocity;
    private Rectangle[] pipes;
    private Random random = new();
    private bool isDone;

    public FlappyBirdGame()
    {
        Initialize();
    }

    private void Initialize()
    {
        bird = new Rectangle(Width / 4, Height / 2, BirdSize, BirdSize);
        birdVelocity = 0;

        pipes = new Rectangle[NumPipes * 2];

        for (var i = 0; i < NumPipes; i++)
        {
            var gapStart = random.Next(Height / 4, 3 * Height / 4);
            pipes[i * 2] = new Rectangle(i * PipeSpacing + Width, 0, PipeWidth, gapStart);
            pipes[i * 2 + 1] = new Rectangle(i * PipeSpacing + Width, gapStart + PipeGap, PipeWidth, Height - gapStart - PipeGap);
        }
    }

    public void HandleInput(IPlayerConsole player1Console)
    {
        // Update bird
        birdVelocity += Gravity;
        bird.Y += birdVelocity;
        bird.Y = Math.Clamp(bird.Y, 0, Height - BirdSize);

        // Flap
        var buttons = player1Console.ReadButtons();
        if (buttons > 0)
        {
            birdVelocity = -FlapPower;
        }
    }

    public void Update()
    {
        // Check for collision
        if (pipes.Any(pipe => bird.IntersectsWith(pipe)))
        {
            isDone = true;
            return;
        }

        // Update pipes
        for (var i = 0; i < pipes.Length; i++)
        {
            pipes[i].X--;

            // Reset pipe when off-screen
            if (pipes[i].Right < 0)
            {
                var gapStart = random.Next(Height / 4, 3 * Height / 4);
                pipes[i] = new Rectangle((i / 2) * PipeSpacing + Width, pipes[i].Y < Height / 2 ? 0 : gapStart + PipeGap, PipeWidth, pipes[i].Height);
            }
        }
    }

    public void Draw(IDisplay display)
    {
        // Draw bird
        display.DrawRectangle(bird.X, bird.Y, BirdSize, BirdSize, Color.Yellow);

        // Draw pipes
        foreach (var pipe in pipes)
        {
            display.DrawRectangle(pipe.X, pipe.Y, PipeWidth, pipe.Height, Color.Green);
        }
    }

    public GameOverState State => isDone ? GameOverState.EndOfGame : GameOverState.None;
}