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
        private int[] buildingHeights;
        private int score;
        private GameOverState gameState;
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
            gameState = GameOverState.None;

            buildingHeights = new int[Buildings];
            var random = new Random();

            for (var i = 0; i < Buildings; i++)
            {
                buildingHeights[i] = random.Next(MinBuildingHeight, MaxBuildingHeight + 1);
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
            if (gameState != GameOverState.None)
            {
                return;
            }

            planeX += PlaneXSpeed;

            if (planeX >= DisplayWidth - plane.Width)
            {
                planeX = 0;
                planeY += PlaneYSpeed;
            }

            if (bombDropped)
            {
                bombY++;

                if (bombY >= DisplayHeight - buildingHeights[bombX] * 8)
                {
                    buildingHeights[bombX]--;
                    bombDropped = false;
                    score++;
                }
            }

            if (planeY >= DisplayHeight - buildingHeights[planeX / 8] * 8)
            {
                gameState = GameOverState.EndOfGame;
            }
            else if (planeY >= DisplayHeight - 8)
            {
                gameState = GameOverState.Player1Wins;
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
                
                for (int y = 0; y < buildingHeights[i]; y++)
                {
                    int buildingY = DisplayHeight - (y + 1) * building.Height;
                    building.Draw(display, i * building.Width, buildingY, y <  buildingHeights[i] - 1 ? 0 : 1);
                }
            }

            // Draw plane
            plane.Draw(display, planeX, planeY);

            // Draw bomb
            if (bombDropped)
            {
                bomb.Draw(display, bombX * 8, bombY);
            }
        }

        public GameOverState State => gameState;
    }
}