using System.Drawing;
using Core;
using Core.Display;
using Core.Display.Sprites;

namespace FlappyBird.Bits;

public class PipeColumn : IGameElement
{
    private const int Width = 64;
    private const int Height = 64;
    private const int PipeWidth = 6;
    
    private static readonly Random Random = new();
    private readonly Pipe topPipe;
    private readonly Pipe bottomPipe;
    
    public PipeColumn(SpriteAnimation greenPipeSprite, SpriteAnimation orangePipeSprite, int pipeNo, int x, int pipeGap)
    {
        var gapStart = Random.Next(greenPipeSprite.Height, Height - pipeGap - orangePipeSprite.Height);
        var sprite = pipeNo % 2 == 0 ? greenPipeSprite : orangePipeSprite;
        topPipe = new Pipe(x, 0, PipeWidth, gapStart, true, sprite);
        bottomPipe = new Pipe(x, gapStart + pipeGap, PipeWidth, Height - gapStart - pipeGap, false, sprite);
    }
    
    public bool IsOffScreen()
    {
        return topPipe.Rectangle.Right < 0;
    }
    
    public bool IsBirdColliding(Rectangle bird)
    {
        return topPipe.Rectangle.IntersectsWith(bird) || bottomPipe.Rectangle.IntersectsWith(bird);
    }
    
    public int Right => topPipe.Rectangle.Right;
    
    public void Update()
    {
        topPipe.Update();
        bottomPipe.Update();
    }

    public void Draw(IDisplay display)
    {
        topPipe.Draw(display);
        bottomPipe.Draw(display);
    }
}