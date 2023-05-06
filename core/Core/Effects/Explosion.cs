using Core.Display.Sprites;

namespace Core.Effects;

public class Explosion : IGameElement
{
    private readonly ISprite sprite;
    private readonly Action? peakEffect;
    private readonly Action? endEffect;
    private int frame;

    public Explosion(ISprite sprite, int x, int y, Action? peakEffect, Action? endEffect)
    {
        this.sprite = sprite;
        this.X = x;
        this.Y = y;
        this.frame = 0;
        this.peakEffect = peakEffect;
        this.endEffect = endEffect;
    }
    
    public int X { get; }
    public int Y { get; }
    public bool IsDone => frame >= 7;
    
    public void Update()
    {
        frame++;
        if (frame == 3 && peakEffect != null)
        {
            peakEffect();
        }
        else if (frame >= 6 && endEffect != null)
        {
            endEffect();
        }
    }

    public void Draw(IDisplay display)
    {
        sprite.Draw(display, X - sprite.Width / 2, Y - sprite.Height / 2, frame);
    }
}