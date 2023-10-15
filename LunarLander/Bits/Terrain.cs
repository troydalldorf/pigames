using System.Drawing;
using Core;
using Core.Display;

namespace LunarLander.Bits;

public class Terrain : IGameElement
{
    private List<int> _heights;
    private LandingPad _landingPad;

    public Terrain()
    {
        _heights = new List<int>();
        Random random = new Random();

        // Generate 5 to 10 random heights
        int pointCount = random.Next(5, 11);
        for (int i = 0; i < pointCount; i++)
        {
            _heights.Add(random.Next(10, 55)); // Values between 10 and 55 for some margin at top and bottom
        }

        // Place the landing pad at a random point on the terrain
        int padX = random.Next(10, 54); // Values between 10 and 54 for some margin on the sides
        int padY = GetHeightAt(padX);
        _landingPad = new LandingPad(padX, padY);
    }

    private int GetHeightAt(int x)
    {
        // Interpolate the height at x
        double pos = (double)x / 64 * (_heights.Count - 1);
        int idx = (int)pos;
        double t = pos - idx;

        int y0 = _heights[idx];
        int y1 = _heights[Math.Min(idx + 1, _heights.Count - 1)];

        return (int)Math.Round((1 - t) * y0 + t * y1);
    }

    public bool IsCollidingWith(int x, int y)
    {
        return y >= GetHeightAt(x);
    }

    public void Update()
    {
    }

    public void Draw(IDisplay display)
    {
        for (int x = 0; x < display.Width; x++)
        {
            int height = GetHeightAt(x);

            // Draw the terrain from the height to the bottom of the screen
            for (int y = height; y < display.Height; y++)
            {
                display.SetPixel(x, y, Color.Brown);
            }
        }

        // Draw the landing pad
        _landingPad.Draw(display);
    }

    public LandingPad GetLandingPad()
    {
        return _landingPad;
    }
}