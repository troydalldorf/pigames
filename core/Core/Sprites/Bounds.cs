namespace Core.Display.Sprites;

public class Bounds
{
    public Bounds(int x1, int y1, int x2, int y2)
    {
        X1 = x1;
        Y1 = y1;
        X2 = x2;
        Y2 = y2;
    }
    
    public int X1 { get; private set; }
    public int Y1 { get; private set; }
    public int X2 { get; private set; }
    public int Y2 { get; private set; }

    public bool Intersects(Bounds otherThingBounds)
    {
        // Check if the two bounds objects intersect
        var intersects = !(X2 < otherThingBounds.X1 || X1 > otherThingBounds.X2 || Y2 < otherThingBounds.Y1 || Y1 > otherThingBounds.Y2);
        return intersects;
    }
}