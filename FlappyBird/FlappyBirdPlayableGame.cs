using Core;
using Core.Display.Fonts;
using Core.Display.Sprites;
using Core.Fonts;
using FlappyBird.Bits;

namespace FlappyBird;

using System;
using System.Drawing;

public class FlappyBirdPlayableGame : IPlayableGameElement
{
    private const int Width = 64;
    private const int Height = 64;
    private const int BirdSize = 8;
    private const int PipeWidth = 8;
    private const int PipeGapStart = 24;
    private int pipeGap = PipeGapStart;
    private const int NumPipes = 3;
    private const int PipeSpacing = Width / NumPipes;
    private const int Gravity = 1;
    private const int FlapPower = 4;
    private int score;
    private Rectangle bird;
    private int birdVelocity;
    private Pipe[] pipes;
    private readonly Random random = new();
    private bool isDone;
    private readonly IFont font;
    private readonly Sprite birdSprite;
    private readonly SpriteAnimation orangePipeSprite;
    private readonly SpriteAnimation greenPipeSprite;

    public FlappyBirdPlayableGame(IFontFactory fontFactory)
    {
        font = fontFactory.GetFont(LedFontType.FontTomThumb);
        var image = SpriteImage.FromResource("flappy.png");
        birdSprite = image.GetSprite(1, 1, 9, 8);
        greenPipeSprite = image.GetSpriteAnimation(1, 9, 8, 8, 2, 1);
        orangePipeSprite = image.GetSpriteAnimation(1, 18, 8, 8, 2, 1);
        Initialize();
    }

    private void Initialize()
    {
        bird = new Rectangle(Width / 4, Height / 2, BirdSize, BirdSize);
        birdVelocity = 0;

        pipes = new Pipe[NumPipes * 2];

        for (var i = 0; i < NumPipes; i++)
        {
            var gapStart = random.Next(Height / 4, 3 * Height / 4);
            var sprite = i % 4 < 2 ? greenPipeSprite : orangePipeSprite;
            pipes[i * 2] = new Pipe(i * PipeSpacing + Width, 0, PipeWidth, gapStart, true, sprite);
            pipes[i * 2 + 1] = new Pipe(i * PipeSpacing + Width, gapStart + pipeGap, PipeWidth, Height - gapStart - pipeGap, false, sprite);
        }

        score = 0;
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
        if (pipes.Any(pipe => bird.IntersectsWith(pipe.Rectangle)))
        {
            isDone = true;
            return;
        }

        // Update pipes
        for (var i = 0; i < pipes.Length; i++)
        {
            pipes[i].Update();

            // Reset pipe when off-screen
            if (pipes[i].Rectangle.Right < 0)
            {
                var gapStart = random.Next(Height / 4, 3 * Height / 4);
                var x = (i / 2) * PipeSpacing + Width;
                var sprite = i % 4 < 2 ? greenPipeSprite : orangePipeSprite;
                if (pipes[i].IsTop)
                {
                    pipes[i] = new Pipe(x, 0, PipeWidth, gapStart, true, sprite);
                }
                else
                {
                    pipes[i] = new Pipe(x, gapStart + pipeGap, PipeWidth, Height - gapStart - pipeGap, false, sprite);
                }
            }
        }

        // Increase score if bird passes through a pair of pipes
        if (pipes.Any(pipe => pipe.X + PipeWidth == bird.X))
        {
            score++;
        }

        // Make gaps larger at the start and reduce them over time
        if (score % 10 == 9 && pipeGap > BirdSize * 4)
        {
            pipeGap--;
        }
    }

    public void Draw(IDisplay display)
    {
        // Draw bird
        birdSprite.Draw(display, bird.X, bird.Y);

        // Draw pipes
        foreach (var pipe in pipes)
        {
            var r = pipe.Rectangle;
            display.DrawRectangle(r.X, r.Y, PipeWidth, r.Height, Color.Green, Color.DarkGreen);
            display.DrawLine(r.X + 1, r.Y + 1, pipe.X + 1, r.Y + r.Height - 1, Color.LawnGreen);
            //pipe.Draw(display);
        }

        // Draw score
        font.DrawText(display, Width-8, 10, Color.DarkOliveGreen, score.ToString());
    }

    public GameOverState State => isDone ? GameOverState.EndOfGame : GameOverState.None;
}