using Core;
using System.Drawing;
using Core.Display.Sprites;
using Frogger.Bits;

namespace Frogger
{
    public class FroggerPlayableGame : IPlayableGameElement
    {
        private const int Width = 64;
        private const int Height = 64;
        private const int NumLanes = 5;

        private readonly Frog frog;
        private List<Lane> lanes;
        private readonly Random random;
        private bool isDone;

        public FroggerPlayableGame()
        {
            random = new Random();
            frog = new Frog(Width, Height);
            InitializeLanes();
        }

        private void InitializeLanes()
        {
            lanes = new List<Lane>();
            for (var i = 0; i < NumLanes; i++)
            {
                lanes.Add(new Lane(Width, Height, i, random));
            }
        }

        public void HandleInput(IPlayerConsole player1Console)
        {
            var stick = player1Console.ReadJoystick();
            frog.HandleInput(stick);

            if (frog.IsAtTop)
            {
                InitializeLanes();
                frog.ResetPosition();
            }
        }

        public void Update()
        {
            foreach (var lane in lanes)
            {
                lane.Update();

                if (lane.CheckCollision(frog.Rectangle))
                {
                    isDone = true;
                    return;
                }
            }
        }

        public void Draw(IDisplay display)
        {
            frog.Draw(display);

            foreach (var lane in lanes)
            {
                lane.Draw(display);
            }
        }

        public GameOverState State => isDone ? GameOverState.EndOfGame : GameOverState.None;
    }

    public class Frog
    {
        private const int FrogSize = 4;
        private const int LaneHeight = 8;
        private const int Width = 64;
        private const int Height = 64;

        private Rectangle rectangle;
        private Sprite frogSprite;
        public bool IsAtTop => rectangle.Y == 0;

        public Frog(int screenWidth, int screenHeight)
        {
            var image = SpriteImage.FromResource("frogger.png", new Point(1, 1));
            frogSprite = image.GetSprite(1, 1, 4, 4);
            ResetPosition();
        }

        public void HandleInput(JoystickDirection stick)
        {
            if (stick.IsUp()) rectangle.Y--;
            if (stick.IsDown()) rectangle.Y++;
            if (stick.IsLeft()) rectangle.X--;
            if (stick.IsRight()) rectangle.X++;

            rectangle.X = Math.Clamp(rectangle.X, 0, Width - FrogSize);
            rectangle.Y = Math.Clamp(rectangle.Y, 0, Height - LaneHeight);
        }

        public void ResetPosition()
        {
            rectangle = new Rectangle(Width / 2 - FrogSize / 2, Height - LaneHeight, FrogSize, FrogSize);
        }

        public void Draw(IDisplay display)
        {
            frogSprite.Draw(display, rectangle.X, rectangle.Y);
        }

        public Rectangle Rectangle => rectangle;
    }
}