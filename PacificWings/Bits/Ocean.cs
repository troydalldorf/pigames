using System.Drawing;
using Core;

namespace PacificWings.Bits
{
    public class Ocean : IGameElement
    {
        private const int NumSquiggles = 25;
        private const int SquiggleSpeed = 1;
        private readonly Color darkBlue = Color.FromArgb(0, 0, 10);
        private readonly List<Squiggle> squiggles;
        private readonly Random random;

        public Ocean()
        {
            squiggles = new List<Squiggle>(NumSquiggles);
            random = new Random();
            GenerateSquiggles();
        }

        private void GenerateSquiggles()
        {
            for (int i = 0; i < NumSquiggles; i++)
            {
                int x = random.Next(0, 64);
                int y = random.Next(0, 64);
                int length = random.Next(3, 10);
                int r = random.Next(0, 128);
                int g = random.Next(128, 256);
                int b = 10;
                Color color = Color.FromArgb(r, g, b);
                squiggles.Add(new Squiggle(x, y, length, color));
            }
        }


        public void Update()
        {
            foreach (var squiggle in squiggles)
            {
                squiggle.Y += SquiggleSpeed;
                if (squiggle.Y > 64)
                {
                    squiggle.Y = 0;
                    squiggle.X = random.Next(0, 64);
                }
            }
        }

        public void Draw(IDisplay display)
        {
            display.DrawRectangle(0, 0, 64, 64, darkBlue, darkBlue);
            foreach (var squiggle in squiggles)
            {
                display.DrawLine(squiggle.X, squiggle.Y, squiggle.X, squiggle.Y + squiggle.Length, squiggle.Color);
            }
        }

        private class Squiggle
        {
            public int X { get; set; }
            public int Y { get; set; }
            public int Length { get; set; }
            public Color Color { get; set; }

            public Squiggle(int x, int y, int length, Color color)
            {
                X = x;
                Y = y;
                Length = length;
                Color = color;
            }
        }
    }
}