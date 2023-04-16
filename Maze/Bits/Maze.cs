using System.Drawing;

namespace Maze.Bits;

public class Maze
{
    public int Width { get; private set; }
    public int Height { get; private set; }
    private readonly bool[,] maze;

    public Maze(int width, int height)
    {
        Width = width;
        Height = height;
        maze = new bool[Width, Height];
        GenerateMaze();
    }

    private void GenerateMaze()
    {
        // Initialize maze array
        for (int x = 0; x < Width; x++)
        {
            for (int y = 0; y < Height; y++)
            {
                maze[x, y] = true;
            }
        }

        // Start generating maze from top-left corner
        Random rand = new Random();
        Stack<Point> stack = new Stack<Point>();
        Point start = new Point(1, 1);
        stack.Push(start);
        maze[start.X, start.Y] = false;

        // Helper function to get neighbors
        List<Point> GetNeighbors(Point p)
        {
            var neighbors = new List<Point>
            {
                new Point(p.X - 2, p.Y),
                new Point(p.X + 2, p.Y),
                new Point(p.X, p.Y - 2),
                new Point(p.X, p.Y + 2),
            };
            neighbors.RemoveAll(pt => pt.X < 0 || pt.X >= Width || pt.Y < 0 || pt.Y >= Height);
            return neighbors;
        }

        // Helper function to check if a cell is visited
        bool IsVisited(Point p) => !maze[p.X, p.Y];

        // Helper function to carve a path between two cells
        void CarvePath(Point from, Point to)
        {
            maze[to.X, to.Y] = false;
            maze[(from.X + to.X) / 2, (from.Y + to.Y) / 2] = false;
        }

        // Main maze generation loop
        while (stack.Count > 0)
        {
            var current = stack.Peek();
            var neighbors = GetNeighbors(current).Where(n => !IsVisited(n)).ToList();

            if (neighbors.Count == 0)
            {
                // Backtrack
                stack.Pop();
            }
            else
            {
                // Choose a random unvisited neighbor and carve a path
                Point next = neighbors[rand.Next(neighbors.Count)];
                CarvePath(current, next);

                // Move to the next cell and mark it as visited
                stack.Push(next);
            }
        }
    }


    public bool IsWall(int x, int y)
    {
        if (x < 0 || x >= Width || y < 0 || y >= Height)
        {
            return true;
        }
        return maze[x, y];
    }
}