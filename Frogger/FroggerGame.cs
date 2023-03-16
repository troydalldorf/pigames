using Core.Display;
using Core.Inputs;

namespace Frogger;

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Threading;

class FroggerGame
{
    private const int Width = 64;
    private const int Height = 64;
    private const int FrogSize = 4;
    private const int VehicleWidth = 8;
    private const int VehicleHeight = 4;
    private const int NumLanes = 5;
    private const int LaneHeight = VehicleHeight + 2;
    private const int MaxSpeed = 3;

    private LedDisplay display;
    private PlayerConsole playerConsole;
    private Rectangle frog;
    private List<Rectangle> vehicles;
    private int[] laneSpeeds;
    private Random random;

    public FroggerGame(LedDisplay display, PlayerConsole playerConsole)
    {
        this.display = display;
        this.playerConsole = playerConsole;
        random = new Random();
    }

    public void Run()
    {
        Initialize();

        while (true)
        {
            Update();
            Draw();
            Thread.Sleep(100);
        }
    }

    private void Initialize()
    {
        frog = new Rectangle(Width / 2 - FrogSize / 2, Height - LaneHeight, FrogSize, FrogSize);

        vehicles = new List<Rectangle>();
        laneSpeeds = new int[NumLanes];

        for (int i = 0; i < NumLanes; i++)
        {
            laneSpeeds[i] = random.Next(1, MaxSpeed + 1) * (random.Next(2) == 0 ? 1 : -1);
            int numVehicles = random.Next(2, 5);
            for (int j = 0; j < numVehicles; j++)
            {
                int x = (Width / numVehicles) * j;
                int y = LaneHeight * i;
                vehicles.Add(new Rectangle(x, y, VehicleWidth, VehicleHeight));
            }
        }
    }

    private void Update()
    {
        // Update frog
        var stick = playerConsole.ReadJoystick();
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
            return;
        }

        // Update vehicles
        for (int i = 0; i < vehicles.Count; i++)
        {
            vehicles[i] = new Rectangle(vehicles[i].X + laneSpeeds[i / 2], vehicles[i].Y, vehicles[i].Width, vehicles[i].Height);

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
                Initialize();
                return;
            }
        }
    }

    private void Draw()
    {
        display.Clear();

        // Draw frog
        display.DrawRectangle(frog.X, frog.Y, FrogSize, FrogSize, Color.Green);

        // Draw
        foreach (var vehicle in vehicles)
        {
            display.DrawRectangle(vehicle.X, vehicle.Y, VehicleWidth, VehicleHeight, Color.Red);
        }

        display.Update();
    }
}