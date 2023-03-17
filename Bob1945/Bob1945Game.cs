using Core.Inputs;
using Core.Display;
using System.Drawing;

namespace _1945
{
    public class Plane
    {
        private const int Speed = 3;
        private const int MaxAltitude = 48;
        private const int MinAltitude = 16;
        private const int MaxHP = 5;
        public bool IsPoweredUp { get; set; }
        private int powerUpTimeRemaining;
        
        public int X { get; set; }
        public int Y { get; set; }
        public int Altitude { get; set; }
        public int HP { get; set; }
        public bool IsAlive { get; set; }

        public Plane(int x, int y)
        {
            X = x;
            Y = y;
            Altitude = MaxAltitude;
            HP = MaxHP;
            IsAlive = true;
        }

        public void Move(Direction direction)
        {
            switch (direction)
            {
                case Direction.Up:
                    Altitude = Math.Max(Altitude - Speed, MinAltitude);
                    break;
                case Direction.Down:
                    Altitude = Math.Min(Altitude + Speed, MaxAltitude);
                    break;
                case Direction.Left:
                    X = Math.Max(X - Speed, 0);
                    break;
                case Direction.Right:
                    X = Math.Min(X + Speed, 64);
                    break;
            }
        }

        public void TakeDamage()
        {
            HP--;
            if (HP <= 0)
            {
                IsAlive = false;
            }
        }
    }

    public class Enemy
    {
        private const int Speed = 2;

        public int X { get; set; }
        public int Y { get; set; }
        public bool IsAlive { get; set; }

        public Enemy(int x, int y)
        {
            X = x;
            Y = y;
            IsAlive = true;
        }

        public void Move()
        {
            X -= Speed;
        }

        public void TakeDamage()
        {
            IsAlive = false;
        }
    }

    public class Projectile
    {
        private const int Speed = 6;

        public int X { get; set; }
        public int Y { get; set; }
        public bool IsAlive { get; set; }
        public bool IsEnemy { get; set; }

        public Projectile(int x, int y, bool isEnemy)
        {
            X = x;
            Y = y;
            IsAlive = true;
            IsEnemy = isEnemy;
        }

        public void Move()
        {
            Y += IsEnemy ? Speed : -Speed;
        }

        public void Die()
        {
            IsAlive = false;
        }
    }

    public class PowerUp
    {
        private const int Speed = 2;

        public int X { get; set; }
        public int Y { get; set; }
        public bool IsAlive { get; set; }
        public PowerUpType Type { get; set; }

        public PowerUp(int x, int y)
        {
            X = x;
            Y = y;
            IsAlive = true;
            Type = (PowerUpType)new Random().Next(3);
        }

        public void Move()
        {
            Y += Speed;
        }

        public enum PowerUpType
        {
            Ammo,
            WeaponUpgrade,
            Shield
        }
    }

    public enum Direction
    {
        Up,
        Down,
        Left,
        Right
    }

    public class Game
    {
        private const int PlaneSize = 6;
        private const int EnemySize = 6;
        private const int ProjectileSize = 2;
        private const int PowerUpSize = 4;
        private const int MaxEnemies = 5;
        private const int MaxProjectiles = 20;
        private const int MaxPowerUps = 3;

        private LedDisplay display;
        private PlayerConsole player1Console;
        private PlayerConsole player2Console;
        private Plane player1Plane;
        private Plane player2Plane;
        private List<Enemy> enemies;
        private List<Projectile> projectiles;
        private List<PowerUp> powerUps;
        private int score;

        public Game(LedDisplay display, PlayerConsole player1Console, PlayerConsole player2Console)
        {
            this.display = display;
            this.player1Console = player1Console;
            this.player2Console = player2Console;
            player1Plane = new Plane(32 - PlaneSize / 2, 60);
            player2Plane = new Plane(32 - PlaneSize / 2, 4);
            enemies = new List<Enemy>();
            projectiles = new List<Projectile>();
            powerUps = new List<PowerUp>();
            score = 0;
        }

        public void Run()
        {
            while (true)
            {
                HandleInput();
                Update();
                Draw();
                Thread.Sleep(50);
            }
        }

        private void HandleInput()
        {
            var player1Stick = player1Console.ReadJoystick();
            var player1Buttons = player1Console.ReadButtons();
            var player2Stick = player2Console.ReadJoystick();
            var player2Buttons = player2Console.ReadButtons();

            if (player1Stick.IsUp())
                player1Plane.Move(Direction.Up);
            if (player1Stick.IsDown())
                player1Plane.Move(Direction.Down);
            if (player1Stick.IsLeft())
                player1Plane.Move(Direction.Left);
            if (player1Stick.IsRight())
                player1Plane.Move(Direction.Right);
            if (player1Buttons.IsGreenPushed())
                FireProjectile(player1Plane.X + PlaneSize / 2, player1Plane.Y - 2, false);

            if (player2Stick.IsUp())
                player2Plane.Move(Direction.Up);
            if (player2Stick.IsDown())
                player2Plane.Move(Direction.Down);
            if (player2Stick.IsLeft())
                player2Plane.Move(Direction.Left);
            if (player2Stick.IsRight())
                player2Plane.Move(Direction.Right);
            if (player1Buttons.IsGreenPushed())
                FireProjectile(player2Plane.X + PlaneSize / 2, player2Plane.Y + PlaneSize + 2, false);
        }

        private void Update()
        {
            UpdateEnemies();
            UpdateProjectiles();
            UpdatePowerUps();
            CheckCollisions();
            SpawnEnemies();
            SpawnPowerUps();
        }

        private void UpdateEnemies()
        {
            foreach (var enemy in enemies)
            {
                enemy.Move();
            }

            enemies.RemoveAll(e => e.X < -EnemySize);
        }

        private void UpdateProjectiles()
        {
            foreach (var projectile in projectiles)
            {
                projectile.Move();
            }

            projectiles.RemoveAll(p => !p.IsAlive || p.X < 0 || p.X >= 64 || p.Y < 0 || p.Y >= 64);
        }

        private void UpdatePowerUps()
        {
            foreach (var powerUp in powerUps)
            {
                powerUp.Move();
            }

            powerUps.RemoveAll(p => !p.IsAlive || p.X < 0 || p.X >= 64 || p.Y < 0 || p.Y >= 64);
        }

        private void CheckCollisions()
        {
            foreach (var enemy in enemies)
            {
                if (player1Plane.IsAlive && Collides(player1Plane.X, player1Plane.Y, PlaneSize, PlaneSize , enemy.X, enemy.Y, EnemySize, EnemySize))
                {
                    player1Plane.IsAlive = false;
                    break;
                }
                if (player2Plane.IsAlive && Collides(player2Plane.X, player2Plane.Y, PlaneSize, PlaneSize,
                        enemy.X, enemy.Y, EnemySize, EnemySize))
                {
                    player2Plane.IsAlive = false;
                    break;
                }

                foreach (var projectile in projectiles)
                {
                    if (Collides(projectile.X, projectile.Y, ProjectileSize, ProjectileSize,
                            enemy.X, enemy.Y, EnemySize, EnemySize))
                    {
                        score += 10;
                        enemy.IsAlive = false;
                        projectile.IsAlive = false;
                        break;
                    }
                }
            }

            enemies.RemoveAll(e => !e.IsAlive);

            foreach (var powerUp in powerUps)
            {
                if (player1Plane.IsAlive && Collides(player1Plane.X, player1Plane.Y, PlaneSize, PlaneSize,
                        powerUp.X, powerUp.Y, PowerUpSize, PowerUpSize))
                {
                    //player1Plane.PowerUp();
                    powerUp.IsAlive = false;
                    break;
                }

                if (player2Plane.IsAlive && Collides(player2Plane.X, player2Plane.Y, PlaneSize, PlaneSize,
                        powerUp.X, powerUp.Y, PowerUpSize, PowerUpSize))
                {
                    //player2Plane.PowerUp();
                    powerUp.IsAlive = false;
                    break;
                }
            }

            powerUps.RemoveAll(p => !p.IsAlive);
        }

        private void SpawnEnemies()
        {
            if (enemies.Count >= MaxEnemies) return;
            var random = new Random();
            var x = 64;
            var y = random.Next(0, 64 - EnemySize);
            enemies.Add(new Enemy(x, y));
        }

        private void SpawnPowerUps()
        {
            if (powerUps.Count >= MaxPowerUps) return;
            var random = new Random();
            var x = random.Next(0, 64 - PowerUpSize);
            var y = random.Next(0, 64 - PowerUpSize);
            powerUps.Add(new PowerUp(x, y));
        }

        private void Draw()
        {
            display.Clear();

            foreach (var enemy in enemies)
            {
                display.DrawRectangle(enemy.X, enemy.Y, EnemySize, EnemySize, Color.Red);
            }

            foreach (var projectile in projectiles)
            {
                display.DrawRectangle(projectile.X, projectile.Y, ProjectileSize, ProjectileSize, Color.White);
            }

            foreach (var powerUp in powerUps)
            {
                display.DrawRectangle(powerUp.X, powerUp.Y, PowerUpSize, PowerUpSize, Color.Yellow);
            }

            if (player1Plane.IsAlive)
            {
                display.DrawRectangle(player1Plane.X, player1Plane.Y, PlaneSize, PlaneSize, Color.Blue);
            }

            if (player2Plane.IsAlive)
            {
                display.DrawRectangle(player2Plane.X, player2Plane.Y, PlaneSize, PlaneSize, Color.Green);
            }

            display.Update();
        }

        private void FireProjectile(int x, int y, bool isEnemy)
        {
            if (projectiles.Count < MaxProjectiles)
            {
                projectiles.Add(new Projectile(x, y, isEnemy));
            }
        }

        private bool Collides(int x1, int y1, int w1, int h1, int x2, int y2, int w2, int h2)
        {
            return x1 < x2 + w2 && x1 + w1 > x2 && y1 < y2 + h2 &&
                   y1 + h1 > y2;
        }
    }
}