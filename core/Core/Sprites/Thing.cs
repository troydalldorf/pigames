using Core.Display;
using Core.Display.Sprites;

namespace Core.Sprites;

public class Thing
{
    public Thing(string name, ISprite sprite, int x, int y)
    {
        Name = name;
        Sprite = sprite;
        X = x;
        Y = y;
    }
    public string Name { get; }
    public int X { get; set; }
    public int Y { get; set; }
    public int Width => Sprite.Width;
    public int Height => Sprite.Height;
    public ISprite Sprite { get; private set; }
    public int SpriteFrameNo { get; set; }
    public Bounds Bounds => new Bounds(X, Y, X+Sprite.Width-1, Y+Sprite.Height-1);
    public bool IsAfter(int x) => Bounds.X2 > x;
    public bool IsBefore(int x) => Bounds.X1 < x;
    public bool IsAbove(int y) => Bounds.Y1 < y;
    public bool IsBelow(int y) => Bounds.Y2 > y;

    public IEnumerable<Collision> GetCollisions(IEnumerable<Thing> otherThings)
    {
        return otherThings
            .Select(GetCollision)
            .Where(c => c != null)
            .Select(c => c ?? throw new InvalidCastException());
    }

    public Collision? GetCollision(Thing otherThing)
    {
        if (!this.Bounds.Intersects(otherThing.Bounds)) return null;
        return new Collision(this, otherThing);
    }

    public void Draw(IDisplay display)
    {
        Sprite.Draw(display, X, Y, SpriteFrameNo);
    }
}