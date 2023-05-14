using System.Drawing;
using Core;

namespace LunarLander.Bits;

public class Terrain : IGameElement
{
    private readonly List<int> _heights;

    public Terrain()
    {
        _heights = new List<int>();

        // Generate random terrain
        var random = new Random();
        for (int i = 0; i < 64; i++)
        {
            _heights.Add(random.Next(5, 15)); // change these numbers to adjust the height of the terrain
        }
    }

    public bool IsCollidingWith(int x, int y)
    {
        // Check for collision with terrain
        return y >= _heights[x];
    }

    public void Update()
    {
        /*...*/
    }

    public void Draw(IDisplay display)
    {
        // Draw the terrain
        for (int i = 0; i < 64; i++)
        {
            display.DrawLine(i, 64 - _heights[i], i, 64, Color.White);
        }
    }
}