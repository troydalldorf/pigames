using Core;
using Core.Display;
using Core.Display.Fonts;
using Core.Display.Sprites;
using Core.Fonts;
using FlappyBird.Bits;

namespace FlappyBird;

using System;
using System.Drawing;

public class FlappyBirdGame : IPlayableGameElement
{
    private const int Width = 64;
    private const int Height = 64;
    private const int BirdSize = 4;
    private const int PipeWidth = 6;
    private const int PipeGapStart = 32;
    private int pipeGap = PipeGapStart;
    private const int NumPipes = 3;
    private const int PipeSpacing = Width / NumPipes;
    private const int Gravity = 1;
    private const int FlapPower = 4;
    private int score;
    private Rectangle bird;
    private int birdVelocity;
    private PipeColumn[] pipes;
    private int pipeNo = 0;
    private readonly Random random = new();
    private bool isDone;
    private readonly IFont font;
    private readonly Sprite birdSprite;
    private readonly SpriteAnimation orangePipeSprite;
    private readonly SpriteAnimation greenPipeSprite;

    public FlappyBirdGame(IFontFactory fontFactory)
    {
        font = fontFactory.GetFont(LedFontType.FontTomThumb);
        var image = SpriteImage.FromResource("flappy.png");
        birdSprite = image.GetSprite(1, 1, 9, 8);
        greenPipeSprite = image.GetSpriteAnimation(1, 10, 8, 8, 3, 1);
        orangePipeSprite = image.GetSpriteAnimation(1, 19, 8, 8, 3, 1);
        Initialize();
    }

    private void Initialize()
    {
        bird = new Rectangle(Width / 4, Height / 2, BirdSize, BirdSize);
        birdVelocity = 0;
        pipes = new PipeColumn[NumPipes];

        for (var i = 0; i < NumPipes; i++)
        {
            pipes[i] = new PipeColumn(greenPipeSprite, orangePipeSprite, pipeNo++, i * PipeSpacing + Width, pipeGap);
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
        if (pipes.Any(pipe => pipe.IsBirdColliding(bird)))
        {
            isDone = true;
            return;
        }

        // Update pipes
        for (var i = 0; i < pipes.Length; i++)
        {
            pipes[i].Update();
            if (pipes[i].IsOffScreen())
                pipes[i] = new PipeColumn(greenPipeSprite, orangePipeSprite, pipeNo++, Width, pipeGap);
        }

        // Increase score if bird passes through a pair of pipes
        if (pipes.Any(pipe => pipe.Right == bird.X))
        {
            score++;
            // Make gaps larger at the start and reduce them over time
            if (score % 5 == 9 && pipeGap > BirdSize * 4)
            {
                pipeGap--;
            }
        }
    }

    public void Draw(IDisplay display)
    {
        birdSprite.Draw(display, bird.X-2, bird.Y-2);
        foreach (var pipe in pipes)
            pipe.Draw(display);
        font.DrawTextWithBorder(display, Width-8, 10, Color.DarkOliveGreen, Color.Black, score.ToString());
    }

    public GameOverState State => isDone ? GameOverState.EndOfGame : GameOverState.None;
}