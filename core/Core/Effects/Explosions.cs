using System.Drawing;
using Core.Display;
using Core.Display.Sprites;

namespace Core.Effects;

public class Explosions : IGameElement
{
    private readonly ISprite sprite;
    private readonly List<Explosion> explosions = new();
    
    public Explosions()
    {
        var image = SpriteImage.FromResource("explode.png", new Point(1, 0));
        this.sprite = image.GetSpriteAnimation(1, 0, 12, 12, 7, 1);
    }

    public void Explode(int x, int y, Action? peakEffect = null, Action? endEffect = null)
    {
        explosions.Add(new Explosion(this.sprite, x, y, peakEffect, endEffect));
    }

    public void Update()
    {
        foreach (var explosion in explosions)
        {
            explosion.Update();
        }
        explosions.RemoveAll(e => e.IsDone);
    }

    public void Draw(IDisplay display)
    {
        foreach (var explosion in explosions)
        {
            explosion.Draw(display);
        }
    }
}