using System.Drawing;
using Core;
using Core.Display.Sprites;

namespace Frogger.Bits;

    public class Lane
    {
        private const int VehicleMinWidth = 6;
        private const int VehicleMaxWidth = 12;
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

        public Lane(int screenWidth, int screenHeight, int index, Random random)
        {
            this.screenWidth = screenWidth;
            this.screenHeight = screenHeight;
            this.index = index;
            this.random = random;

            var image = SpriteImage.FromResource("frogger.png", new Point(1, 1));
            vehicleSprite = image.GetSprite(1, 6, VehicleMaxWidth, VehicleHeight);
            laneSpeed = random.Next(1, MaxSpeed + 1);

            InitializeVehicles();
        }

        private void InitializeVehicles()
        {
            vehicles = new List<Vehicle>();
            int currentX = 0;

            while (currentX < screenWidth)
            {
                var vehicleWidth = random.Next(VehicleMinWidth, VehicleMaxWidth + 1);
                var y = LaneHeight * index;

                vehicles.Add(new Vehicle(currentX, y, vehicleWidth, vehicleSprite));
                currentX += vehicleWidth + random.Next(vehicleWidth / 2, vehicleWidth);
            }
        }

        public void Update()
        {
            for (int i = 0; i < vehicles.Count; i++)
            {
                vehicles[i].Update(laneSpeed);

                if (vehicles[i].Rectangle.Right < 0)
                {
                    int currentX = vehicles[i].Rectangle.X + vehicles[i].Rectangle.Width;
                    int vehicleWidth = random.Next(VehicleMinWidth, VehicleMaxWidth + 1);
                    int y = LaneHeight * index;

                    vehicles.Add(new Vehicle(currentX, y, vehicleWidth, vehicleSprite));
                    vehicles.RemoveAt(i);
                    i--;
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