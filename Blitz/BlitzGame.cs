using System;
using System.Drawing;
using Core;
using Core.Display.Sprites;

namespace Blitz
{
    public class BlitzGame : IPlayableGameElement
    {
        private const int DisplayHeight = 64;
        private const int DisplayWidth = 64;
        private const int Buildings = 8;
        private const int MinBuildingHeight = 1;
        private const int MaxBuildingHeight = 6;
        private const int PlaneXSpeed = 2;
        private const int PlaneYSpeed = 2;
        private int planeX;
        private int planeY;
        private int bombX;
        private int bombY;
        private bool bombDropped;
        private (int,int)[] buildingHeights;
        private int score;
        private readonly ISprite plane;
        private readonly ISprite bomb;
        private readonly ISprite building;
        private readonly ISprite grass;

        public BlitzGame()
        {
            var image = SpriteImage.FromResource("blitz.png", new Point(63, 1));
            this.grass = image.GetSpriteAnimation(1, 1, 4, 4, 3, 1);
            this.bomb = image.GetSpriteAnimation(1, 6, 4, 5, 1, 1);
            this.plane = image.GetSpriteAnimation(1, 12, 13, 5, 2, 1);
            this.building = image.GetSpriteAnimation(1, 18, 8, 8, 3, 1);
            Initialize();
        }

        private void Initialize()
        {
            planeX = 0;
            planeY = 0;
            bombDropped = false;
            score = 0;
            State = GameOverState.None;

            buildingHeights = new (int, int)[Buildings];
            var random = new Random();

            for (var i = 0; i < Buildings; i++)
            {
                var height = random.Next(MinBuildingHeight, MaxBuildingHeight + 1);
                buildingHeights[i] = (height, height);
            }
        }

        public void HandleInput(IPlayerConsole playerConsole)
        {
            var buttons = playerConsole.ReadButtons();

            if (!bombDropped && buttons.HasFlag(Buttons.Green))
            {
                bombDropped = true;
                bombX = (planeX + plane.Width / 2) / 8;
                bombY = planeY;
            }
        }

        public void Update()
        {
            if (State != GameOverState.None)
            {
                return;
            }

            planeX += PlaneXSpeed;

            if (planeX >= DisplayWidth - plane.Width / 2)
            {
                planeX = -plane.Width / 2;
                planeY += PlaneYSpeed;
            }

            if (bombDropped)
            {
                bombY++;

                if (bombY >= DisplayHeight - buildingHeights[bombX].Item1 * 8 - grass.Height)
                {
                    buildingHeights[bombX].Item1--;
                    bombDropped = false;
                    score++;
                }
            }

            if (planeY >= DisplayHeight - buildingHeights[planeX / 8].Item1 * 8 - grass.Height)
            {
                State = GameOverState.EndOfGame;
            }
            else if (planeY >= DisplayHeight - 8)
            {
                State = GameOverState.Player1Wins;
            }
        }

        public void Draw(IDisplay display)
        {
            for (var i = 0; i < DisplayWidth / grass.Width; i++)
            {
                grass.Draw(display, i*grass.Width, DisplayHeight - grass.Height);
            }

            // Draw buildings
            for (var i = 0; i < Buildings; i++)
            {
                
                for (var y = 0; y < buildingHeights[i].Item1; y++)
                {
                    var buildingY = DisplayHeight - (y + 1) * building.Height - grass.Height;
                    var frame = y == buildingHeights[i].Item2-1 ? 2 : y == buildingHeights[i].Item1 - 1 ? 0 : 1;
                    building.Draw(display, i * building.Width, buildingY, frame);
                }
            }

            // Draw plane
            plane.Draw(display, planeX, planeY);

            // Draw bomb
            if (bombDropped)
            {
                bomb.Draw(display, bombX * 8 + 3, bombY);
            }
        }

        public GameOverState State { get; private set; }
    }
}