using System.Drawing;
using Core;
using Core.Display.Sprites;
using System.Linq;

namespace Frogger.Bits
{
    public class Lane
    {
        private const int VehicleMinWidth = 6;
        private const int VehicleMaxWidth = 12;
        private const int MinSpacing = 6;
        private const int MaxSpacing = 24;
        private const int VehicleHeight = 4;
        private const int LaneHeight = VehicleHeight + 2;
        private const int MaxSpeed = 3;

        private List<Vehicle> vehicles;
        private readonly int laneSpeed;
        private readonly Random random;
        private readonly int screenHeight;
        private readonly int screenWidth;
        private readonly int index;
        private readonly ISprite vehicleSprite;
        private readonly bool moveRight;
        private readonly int difficulty;

        public Lane(int screenWidth, int screenHeight, int index, Random random, int difficulty)
        {
            this.screenWidth = screenWidth;
            this.screenHeight = screenHeight;
            this.index = index;
            this.random = random;
            this.difficulty = difficulty;

            var image = SpriteImage.FromResource("frogger.png", new Point(1, 1));
            vehicleSprite = image.GetSprite(1, 6, VehicleMaxWidth, VehicleHeight);
            laneSpeed = random.Next(1, MaxSpeed + 1) * difficulty;
            moveRight = random.Next(2) == 0;

            InitializeVehicles();
        }

        private void InitializeVehicles()
        {
            vehicles = new List<Vehicle>();
            var currentX = moveRight ? 0 : screenWidth;

            while ((moveRight && currentX < screenWidth) || (!moveRight && currentX > 0))
            {
                var vehicleWidth = random.Next(VehicleMinWidth, VehicleMaxWidth + 1);
                var y = LaneHeight * index;
                vehicles.Add(new Vehicle(currentX, y, vehicleWidth, vehicleSprite));

                var spacing = random.Next(MinSpacing * difficulty, MaxSpacing / difficulty + 1);
                currentX += moveRight ? spacing : -spacing;
            }
        }

        public void Update()
        {
            for (var i = 0; i < vehicles.Count; i++)
            {
                vehicles[i].Update(moveRight ? laneSpeed : -laneSpeed);

                if (moveRight && vehicles[i].Rectangle.Right > screenWidth ||
                    !moveRight && vehicles[i].Rectangle.Left < 0)
                {
                    var vehicleWidth = random.Next(VehicleMinWidth, VehicleMaxWidth + 1);
                    var y = LaneHeight * index;
                    var newX = moveRight ? -vehicleWidth : screenWidth;

                    vehicles[i] = new Vehicle(newX, y, vehicleWidth, vehicleSprite);
                }
            }
        }

        public bool CheckCollision(Rectangle frog)
        {
            return vehicles.Any(vehicle => frog.IntersectsWith(vehicle.Rectangle));
        }

        public void Draw(IDisplay display)
        {
            foreach (var vehicle in vehicles)
            {
                vehicle.Draw(display);
            }
        }
    }
}