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
                int intensity = random.Next(15, 40);
                int saturation = random.Next(15, intensity);
                int r = saturation;
                int g = saturation;
                int b = intensity;
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
                display.SetPixel(squiggle.X, squiggle.Y, squiggle.Color);
                display.SetPixel(squiggle.X+1, squiggle.Y-1, squiggle.Color);
                display.SetPixel(squiggle.X+2, squiggle.Y, squiggle.Color);
                display.SetPixel(squiggle.X+3, squiggle.Y+1, squiggle.Color);
                display.SetPixel(squiggle.X, squiggle.Y, squiggle.Color);
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