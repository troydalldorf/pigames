using Core.Display;
using Core.Inputs;
using System.Drawing;

namespace GeometryDash
{
    public class GeometryDashGame
    {
        private const int Width = 64;
        private const int Height = 64;
        private const int PlayerWidth = 2;
        private const int PlayerHeight = 8;
        private const int ObstacleWidth = 5;
        private const int ObstacleHeight = 5;
        private const int ObstacleSpacing = 20;
        private const int JumpHeight = 16;
        private const int Gravity = 2;

        private LedDisplay display;
        private PlayerConsole playerConsole;

        private Rectangle player;
        private List<Rectangle> obstacles;
        private int obstacleOffset;
        private int score;
        private bool gameRunning;

        public GeometryDashGame(LedDisplay display, PlayerConsole playerConsole)
        {
            this.display = display;
            this.playerConsole = playerConsole;

            player = new Rectangle(10, Height / 2 - PlayerHeight / 2, PlayerWidth, PlayerHeight);
            obstacles = new List<Rectangle>();
            obstacleOffset = 0;
            score = 0;
            gameRunning = false;
        }

        public void Run()
        {
            gameRunning = true;

            while (gameRunning)
            {
                HandleInput();
                Update();
                Draw();
                Thread.Sleep(50);
            }
        }

        private void HandleInput()
        {
            var stick = playerConsole.ReadJoystick();

            if (stick.IsUp())
            {
                player.Y -= JumpHeight;
            }
        }

        private void Update()
        {
            // Move player
            player.Y += Gravity;
            if (player.Bottom >= Height) player.Y = Height - player.Height - 1;

            // Spawn new obstacles
            obstacleOffset++;
            if (obstacleOffset % ObstacleSpacing == 0)
            {
                var obstacle = new Rectangle(Width-1, Height - 1 - ObstacleHeight, ObstacleWidth, ObstacleHeight);
                obstacles.Add(obstacle);
            }

            // Move obstacles
            for (var i = 0; i < obstacles.Count; i++)
            {
                var obstacle = obstacles[i];
                obstacle.X -= 2;

                // Check for collision
                if (obstacle.IntersectsWith(player))
                {
                    gameRunning = false;
                }

                // Remove obstacle if it's offscreen
                if (obstacle.Right < 0)
                {
                    obstacles.RemoveAt(i);
                    score++;
                    i--;
                }
            }

            // Check for game over
            if (player.Top < 0)
            {
                gameRunning = false;
            }
        }

        private void Draw()
        {
            display.Clear();

            // Draw player
            display.DrawRectangle(player.X, player.Y, PlayerWidth, PlayerHeight, Color.White);

            // Draw obstacles
            foreach (var obstacle in obstacles)
            {
                display.DrawRectangle(obstacle.X, obstacle.Y, obstacle.Width, obstacle.Height, Color.Red);
            }

            // Draw score
           // display.DrawText($"Score: {score}", 0, 0, Color.White);

            display.Update();
        }
    }
}
