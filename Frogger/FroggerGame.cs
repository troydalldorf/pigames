using Core;
using System.Drawing;

namespace Frogger;

public class FroggerGame : IGameElement
{
    private const int Width = 64;
    private const int Height = 64;
    private const int FrogSize = 4;
    private const int VehicleWidth = 8;
    private const int VehicleHeight = 4;
    private const int NumLanes = 5;
    private const int LaneHeight = VehicleHeight + 2;
    private const int MaxSpeed = 3;

    private Rectangle frog;
    private List<Rectangle> vehicles;
    private int[] laneSpeeds;
    private readonly Random random;
    private bool isDone;
    //private readonly Sprite frogSprite;

    public FroggerGame()
    {
        //var image = new SpriteImage("frogger.png", new Point(0, 1));
        //frogSprite = image.GetSprite(0, 0, 4, 4);
        random = new Random();
        Initialize();
    }

    private void Initialize()
    {
        frog = new Rectangle(Width / 2 - FrogSize / 2, Height - LaneHeight, FrogSize, FrogSize);

        vehicles = new List<Rectangle>();
        laneSpeeds = new int[NumLanes];

        for (var i = 0; i < NumLanes; i++)
        {
            laneSpeeds[i] = random.Next(1, MaxSpeed + 1) * (random.Next(2) == 0 ? 1 : -1);
            var numVehicles = random.Next(2, 5);
            for (var j = 0; j < numVehicles; j++)
            {
                var x = (Width / numVehicles) * j;
                var y = LaneHeight * i;
                vehicles.Add(new Rectangle(x, y, VehicleWidth, VehicleHeight));
            }
        }
    }

    public void HandleInput(IPlayerConsole player1Console)
    {
        // Update frog
        var stick = player1Console.ReadJoystick();
        if (stick.IsUp()) frog.Y--;
        if (stick.IsDown()) frog.Y++;
        if (stick.IsLeft()) frog.X--;
        if (stick.IsRight()) frog.X++;
        frog.X = Math.Clamp(frog.X, 0, Width - FrogSize);
        frog.Y = Math.Clamp(frog.Y, 0, Height - LaneHeight);

        // Check for victory
        if (frog.Y == 0)
        {
            Initialize();
        }
    }

    public void Update()
    {
        // Update vehicles
        for (var i = 0; i < vehicles.Count; i++)
        {
            vehicles[i] = new Rectangle(vehicles[i].X + laneSpeeds[i % laneSpeeds.Length], vehicles[i].Y, vehicles[i].Width, vehicles[i].Height);
            
            // Wrap around
            if (vehicles[i].Right < 0)
            {
                vehicles[i] = new Rectangle(Width, vehicles[i].Y, vehicles[i].Width, vehicles[i].Height);
            }
            else if (vehicles[i].Left > Width)
            {
                vehicles[i] = new Rectangle(-VehicleWidth, vehicles[i].Y, vehicles[i].Width, vehicles[i].Height);
            }

            // Check for collision
            if (frog.IntersectsWith(vehicles[i]))
            {
                isDone = true;
                return;
            }
        }
    }

    public void Draw(IDisplay display)
    {
        display.DrawRectangle(frog.X, frog.Y, FrogSize, FrogSize, Color.Green);
        //frogSprite.Draw(display, frog.X, frog.Y);
        foreach (var vehicle in vehicles)
        {
            display.DrawRectangle(vehicle.X, vehicle.Y, VehicleWidth, VehicleHeight, Color.Red);
        }
    }

    public GameOverState State() => isDone ? GameOverState.EndOfGame : GameOverState.None;
}