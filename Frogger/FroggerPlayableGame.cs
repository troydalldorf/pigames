using Core;
using System;
using System.Collections.Generic;
using System.Drawing;
using Core.Display.Sprites;
using Core.Sprites;

namespace Frogger
{
    public class FroggerPlayableGame : IPlayableGameElement
    {
        private const int Width = 64;
        private const int Height = 64;
        private const int FrogSize = 4;
        private const int VehicleMinWidth = 6;
        private const int VehicleMaxWidth = 12;
        private const int VehicleHeight = 4;
        private const int NumLanes = 5;
        private const int LaneHeight = VehicleHeight + 2;
        private const int MaxSpeed = 3;

        private Rectangle frog;
        private Sprite frogSprite;
        private Sprite vehicleSprite;
        private List<Rectangle> vehicles;
        private int[] laneSpeeds;
        private readonly Random random;
        private bool isDone;

        public FroggerPlayableGame()
        {
            random = new Random();
            var image = SpriteImage.FromResource("frogger.png", new Point(1, 1));
            frogSprite = image.GetSprite(1, 1, 4, 4);
            vehicleSprite = image.GetSprite(1, 6, VehicleMaxWidth, VehicleHeight);
            Initialize();
        }

        private void Initialize()
        {
            frog = new Rectangle(Width / 2 - FrogSize / 2, Height - LaneHeight, FrogSize, FrogSize);

            vehicles = new List<Rectangle>();
            laneSpeeds = new int[NumLanes];

            for (var i = 0; i < NumLanes; i++)
            {
                laneSpeeds[i] = random.Next(1, MaxSpeed + 1);
                int currentX = 0;
                while (currentX < Width)
                {
                    var vehicleWidth = random.Next(VehicleMinWidth, VehicleMaxWidth + 1);
                    var y = LaneHeight * i;
                    vehicles.Add(new Rectangle(currentX, y, vehicleWidth, VehicleHeight));
                    currentX += vehicleWidth + random.Next(vehicleWidth / 2, vehicleWidth);
                }
            }
        }

        public void HandleInput(IPlayerConsole player1Console)
        {
            var stick = player1Console.ReadJoystick();
            if (stick.IsUp()) frog.Y--;
            if (stick.IsDown()) frog.Y++;
            if (stick.IsLeft()) frog.X--;
            if (stick.IsRight()) frog.X++;
            frog.X = Math.Clamp(frog.X, 0, Width - FrogSize);
            frog.Y = Math.Clamp(frog.Y, 0, Height - LaneHeight);

            if (frog.Y == 0)
            {
                Initialize();
            }
        }

        public void Update()
        {
            for (var i = 0; i < vehicles.Count; i++)
            {
                var currentLane = i % laneSpeeds.Length;
                vehicles[i] = new Rectangle(vehicles[i].X + laneSpeeds[currentLane], vehicles[i].Y, vehicles[i].Width, vehicles[i].Height);

                if (vehicles[i].Right < 0)
                {
                    int currentX = vehicles[i].X + vehicles[i].Width;
                    while (currentX < Width)
                    {
                        var vehicleWidth = random.Next(VehicleMinWidth, VehicleMaxWidth + 1);
                        var y = LaneHeight * currentLane;
                        vehicles.Add(new Rectangle(currentX, y, vehicleWidth, VehicleHeight));
                        currentX += vehicleWidth + random.Next(vehicleWidth / 2, vehicleWidth);
                    }
                }
                else if (vehicles[i].Left > Width)
                {
                    vehicles.RemoveAt(i);
                    i--;
                }

                if (frog.IntersectsWith(vehicles[i]))
                {
                    isDone = true;
                    return;
                }
            }
        }

        public void Draw(IDisplay display)
        {
            frogSprite.Draw(display, frog.X, frog.Y);
            foreach (var vehicle in vehicles)
            {
                var srcRectangle = new Rectangle(0, 0, vehicle.Width, vehicle.Height);
                vehicleSprite.DrawTiled(display, srcRectangle);
            }
        }

        public GameOverState State => isDone ? GameOverState.EndOfGame : GameOverState.None;
    }
}