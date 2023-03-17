using Core.Display;
using Core.Inputs;

namespace FlappyBird;

using System;
using System.Drawing;
using System.Threading;

class FlappyBirdGame
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

    private LedDisplay display;
    private PlayerConsole playerConsole;
    private Rectangle bird;
    private int birdVelocity;
    private Rectangle[] pipes;
    private Random random;

    public FlappyBirdGame(LedDisplay display, PlayerConsole playerConsole)
    {
        this.display = display;
        this.playerConsole = playerConsole;
        random = new Random();
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
        bird = new Rectangle(Width / 4, Height / 2, BirdSize, BirdSize);
        birdVelocity = 0;

        pipes = new Rectangle[NumPipes * 2];

        for (int i = 0; i < NumPipes; i++)
        {
            int gapStart = random.Next(Height / 4, 3 * Height / 4);
            pipes[i * 2] = new Rectangle(i * PipeSpacing + Width, 0, PipeWidth, gapStart);
            pipes[i * 2 + 1] = new Rectangle(i * PipeSpacing + Width, gapStart + PipeGap, PipeWidth, Height - gapStart - PipeGap);
        }
    }

    private void Update()
    {
        // Update bird
        birdVelocity += Gravity;
        bird.Y += birdVelocity;
        bird.Y = Math.Clamp(bird.Y, 0, Height - BirdSize);

        // Flap
        var buttons = playerConsole.ReadButtons();
        if (buttons > 0)
        {
            birdVelocity = -FlapPower;
        }

        // Check for collision
        foreach (var pipe in pipes)
        {
            if (bird.IntersectsWith(pipe))
            {
                Initialize();
                return;
            }
        }

        // Update pipes
        for (int i = 0; i < pipes.Length; i++)
        {
            pipes[i].X--;

            // Reset pipe when off-screen
            if (pipes[i].Right < 0)
            {
                int gapStart = random.Next(Height / 4, 3 * Height / 4);
                pipes[i] = new Rectangle((i / 2) * PipeSpacing + Width, pipes[i].Y < Height / 2 ? 0 : gapStart + PipeGap, PipeWidth, pipes[i].Height);
            }
        }
    }

    private void Draw()
    {
        display.Clear();

        // Draw bird
        display.DrawRectangle(bird.X, bird.Y, BirdSize, BirdSize, Color.Yellow);

        // Draw pipes
        foreach (var pipe in pipes)
        {
            display.DrawRectangle(pipe.X, pipe.Y, PipeWidth, pipe.Height, Color.Green);
        }

        display.Update();
    }
}