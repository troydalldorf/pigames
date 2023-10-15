using Core;
using Core.Display;
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
        private int difficulty;

        public FroggerPlayableGame()
        {
            random = new Random();
            frog = new Frog(Width, Height);
            difficulty = 1;
            InitializeLanes();
        }

        private void InitializeLanes()
        {
            lanes = new List<Lane>();
            for (var i = 0; i < NumLanes; i++)
            {
                lanes.Add(new Lane(Width, Height, i, random, difficulty));
            }
        }

        public void HandleInput(IPlayerConsole player1Console)
        {
            var stick = player1Console.ReadJoystick();
            frog.HandleInput(stick);

            if (frog.IsAtTop)
            {
                difficulty++;
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
}