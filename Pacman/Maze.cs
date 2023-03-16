using System.Drawing;
using Core.Display;

namespace Pacman;

public class Maze
{
    private const int TileSize = 4;
    private const char Wall = '#';
    private const char Empty = ' ';
    
    private string[] mazeData = new string[]
    {
        "####################",
        "#                  #",
        "# ## ##### ## ##### #",
        "# ## ##### ## ##### #",
        "# ## ##### ## ##### #",
        "#                  #",
        "# ##### ##### ##### #",
        "# ##### ##### ##### #",
        "# ##### ##### ##### #",
        "#                  #",
        "####################",
    };

    public bool IsWall(int x, int y)
    {
        var tileX = x / TileSize;
        var tileY = y / TileSize;

        if (tileY < 0 || tileY >= mazeData.Length || tileX < 0 || tileX >= mazeData[tileY].Length)
        {
            return true;
        }

        return mazeData[tileY][tileX] == Wall;
    }

    public void Draw(LedDisplay display)
    {
        for (int y = 0; y < mazeData.Length; y++)
        {
            for (int x = 0; x < mazeData[y].Length; x++)
            {
                if (mazeData[y][x] == Wall)
                {
                    display.DrawRectangle(x * TileSize, y * TileSize, TileSize, TileSize, Color.Blue);
                }
            }
        }
    }
}