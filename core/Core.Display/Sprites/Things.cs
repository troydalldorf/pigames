using System.Collections;

namespace Core.Display.Sprites;

public class Things : Things<Thing> { }

public class Things<T>: IEnumerable<T>
    where T: Thing
{
    private List<T> list = new();
    
    public void Add(T thing)
    {
        list.Add(thing);
    }

    public bool Remove(T thing)
    {
        return list.Remove(thing);
    }

    private void Visit(Action<T> action)
    {
        foreach (var thing in list)
            action(thing);
    }

    public void SetAllFrameNo(int spriteFrameNo)
    {
        Visit(t => t.SpriteFrameNo = spriteFrameNo);
    }

    public void MoveAll(int deltaX, int deltaY)
    {
        Visit(t =>
        {
            t.X += deltaX;
            t.Y += deltaY;
        });
    }

    public bool AreAnyAfter(int x)
    {
        return list.Any(t => t.Bounds.X2 > x);
    }
    
    public bool AreAnyBefore(int x)
    {
        return list.Any(t => t.Bounds.X1 < x);
    }
    
    public bool AreAnyAbove(int y)
    {
        return list.Any(t => t.Bounds.X1 < y);
    }
    
    public bool AreAnyBelow(int y)
    {
        return list.Any(t => t.Bounds.X1 < y);
    }
    
    public IEnumerable<Collision> GetCollisions(IEnumerable<Thing> otherThings)
    {
        return list.SelectMany(t => t.GetCollisions(otherThings));
    }

    public void DrawAll(LedDisplay display)
    {
        Visit(t => t.Draw(display));
    }

    public IEnumerator<T> GetEnumerator()
    {
        return list.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}