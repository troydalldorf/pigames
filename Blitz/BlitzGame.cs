using System.Drawing;
using Core;
using Core.Display.Sprites;
using Core.Effects;
using Core.Fonts;

namespace Blitz
{
    public class BlitzGame : IPlayableGameElement
    {
        private const int DisplayHeight = 64;
        private const int DisplayWidth = 64;
        private const int Buildings = 8;
        private const int MinBuildingHeight = 2;
        private const int MaxBuildingHeight = 7;
        private int planeXSpeed = 2;
        private int planeYSpeed = 2;
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
        private readonly Explosions explosions = new();
        private readonly IFont font;
        private readonly Random random = new();
        private int level = 0;

        public BlitzGame(IFontFactory fontFactory)
        {
            this.font = fontFactory.GetFont(LedFontType.FontTomThumb);
            var image = SpriteImage.FromResource("blitz.png", new Point(63, 1));
            this.grass = image.GetSpriteAnimation(1, 1, 4, 4, 3, 1);
            this.bomb = image.GetSpriteAnimation(1, 6, 4, 5, 1, 1);
            this.plane = image.GetSpriteAnimation(1, 12, 13, 5, 2, 1);
            this.building = image.GetSpriteAnimation(1, 18, 8, 8, 3, 1);
            score = 0;
            State = GameOverState.None;
            NextLevel();
        }

        private void NextLevel()
        {
            planeX = 0;
            planeY = 0;
            bombDropped = false;
            buildingHeights = new (int, int)[Buildings];

            // Level stuff
            level++;
            planeXSpeed = level / 5 + 2;
            planeYSpeed = 2;
            var minBuildingHeight = MinBuildingHeight + level % 5;
            var maxBuildingHeight = MaxBuildingHeight;

            // Calculate target total height for the level
            var targetTotalHeight = Buildings * (minBuildingHeight + maxBuildingHeight) / 2;

            // Create buildings
            var remainingHeight = targetTotalHeight;
            for (var i = 0; i < Buildings; i++)
            {
                var maxHeightThisBuilding = Math.Min(remainingHeight - (Buildings - i - 1) * minBuildingHeight, maxBuildingHeight);
                var minHeightThisBuilding = Math.Min(minBuildingHeight, remainingHeight - (Buildings - i - 1) * maxBuildingHeight);

                var height = random.Next(minHeightThisBuilding, maxHeightThisBuilding + 1);
                remainingHeight -= height;
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

            planeX += planeXSpeed;

            if (planeX >= DisplayWidth - plane.Width / 2)
            {
                planeX = -plane.Width / 2;
                planeY += planeYSpeed;
            }
            
            explosions.Update();

            if (bombDropped)
            {
                bombY++;

                if (bombY >= DisplayHeight - buildingHeights[bombX].Item1 * 8 - grass.Height)
                {
                    if (buildingHeights[bombX].Item1 > 0)
                    {
                        explosions.Explode(bombX * 8 + 4, bombY + 8, () => buildingHeights[bombX].Item1--);
                        score++;
                    }
                    bombDropped = false;
                }
            }

            if (planeY >= DisplayHeight - buildingHeights[planeX / 8].Item1 * 8 - grass.Height)
            {
                State = GameOverState.EndOfGame;
            }
            else if (planeY >= DisplayHeight - 8)
            {
                score += 50;
                NextLevel();
            }
        }

        public void Draw(IDisplay display)
        {
            // Draw plane
            plane.Draw(display, planeX, planeY);

            // Draw bomb
            if (bombDropped)
            {
                bomb.Draw(display, bombX * 8 + 3, bombY);
            }
            
            // Draw buildings
            for (var i = 0; i < Buildings; i++)
            {
                
                for (var y = 0; y < buildingHeights[i].Item1; y++)
                {
                    var buildingY = DisplayHeight - (y + 1) * building.Height;
                    var frame = y == buildingHeights[i].Item2-1 ? 2 : y == buildingHeights[i].Item1 - 1 ? 0 : 1;
                    building.Draw(display, i * building.Width, buildingY, frame);
                }
            }

            // Draw explosions
            explosions.Draw(display);
            
            // Draw grass
            for (var i = 0; i < DisplayWidth / grass.Width; i++)
            {
                grass.Draw(display, i*grass.Width, DisplayHeight - grass.Height);
            }
            
            // Draw score
            font.DrawText(display, 0, 8,  Color.Chartreuse, score.ToString());
        }

        public GameOverState State { get; private set; }
    }

    class Explosion
    {
        public int X { get; set; }
        public int Y { get; set; }
        public int Frame { get; set; }
        public int BombX { get; set; }
    }
}