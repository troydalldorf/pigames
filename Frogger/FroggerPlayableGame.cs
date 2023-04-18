using Core;
using System;
using System.Collections.Generic;
using System.Drawing;
using Core.Display.Sprites;

namespace Frogger
{
    public class FroggerPlayableGame : IPlayableGameElement
    {
        private const int Width = 64;
        private const int Height = 64;
        private const int FrogSize = 4;
        private const int VehicleWidth = 8;
        private const int VehicleHeight = 4;
        private const int NumLanes = 5;
        private const int LaneHeight = VehicleHeight + 2;
        private const int MaxSpeed = 3;
        private const int VehicleSpacing = 16;

        private Rectangle frog;
        private Sprite frogSprite;
        private Sprite vehicleSprite1;
        private Sprite vehicleSprite2;
        private List<Rectangle> vehicles;
        private int uniformSpeed;
        private readonly Random random;
        private bool isDone;

        public FroggerPlayableGame()
        {
            random = new Random();
            var image = SpriteImage.FromResource("frogger.png", new Point(1, 1));
            frogSprite = image.GetSprite(1, 1, 4, 4);
            vehicleSprite1 = image.GetSprite(1, 6, 8, 4);
            vehicleSprite2 = image.GetSprite(1, 11, 8, 4);
            Initialize();
        }

        private void Initialize()
        {
            frog = new Rectangle(Width / 2 - FrogSize / 2, Height - LaneHeight, FrogSize, FrogSize);

            vehicles = new List<Rectangle>();
            uniformSpeed = 1;

            for (var i = 0; i < NumLanes; i++)
            {
                var numVehicles = random.Next(2, 5);
                for (var j = 0; j < numVehicles; j++)
                {
                    var x = (Width / (numVehicles + 1)) * (j + 1);
                    var y = LaneHeight * i;
                    vehicles.Add(new Rectangle(x, y, VehicleWidth, VehicleHeight));
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
                vehicles[i] = new Rectangle(vehicles[i].X + uniformSpeed, vehicles[i].Y, vehicles[i].Width, vehicles[i].Height);

                if (vehicles[i].Right < 0)
                {
                    vehicles[i] = new Rectangle(Width + VehicleSpacing, vehicles[i].Y, vehicles[i].Width, vehicles[i].Height);
                }
                else if (vehicles[i].Left > Width)
                {
                    vehicles[i] = new Rectangle(-VehicleWidth - VehicleSpacing, vehicles[i].Y, vehicles[i].Width, vehicles[i].Height);
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
                vehicleSprite1.Draw(display, vehicle.X, vehicle.Y);
        }

        public GameOverState State => isDone ? GameOverState.EndOfGame : GameOverState.None;
    }
}