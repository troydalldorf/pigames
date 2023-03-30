using BomberMan.Bits;
using Core;
using Core.Display.Fonts;
using Core.Display.Sprites;

namespace BomberMan;

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Threading;

public class BombermanGame : IDuoPlayableGameElement
{
    private const int GridSize = 8;
    private const int BombTimer = 3000;
    private const int ExplosionDuration = 500;
    private readonly LedFont font;

    private Grid grid;
    private Player player1;
    private Player player2;
    private List<Bomb> bombs;
    private List<Explosion> explosions;
    private Sprite p1Sprite;
    private Sprite p2Sprite;
    private Sprite bombSprite;

    public BombermanGame()
    {
        font = new LedFont(LedFontType.Font4x6);
        var image = SpriteImage.FromResource("bm.png");
        p1Sprite = image.GetSprite(1, 1, 8, 8);
        p2Sprite = image.GetSprite(10, 1, 8, 8);
        bombSprite = image.GetSprite(19, 1, 8, 8);
        Initialize();
    }

    private void Initialize()
    {
        grid = new Grid(GridSize, GridSize);
        player1 = new Player(0, 0, grid, p1Sprite);
        player2 = new Player(GridSize - 1, GridSize - 1, grid, p2Sprite);
        bombs = new List<Bomb>();
        explosions = new List<Explosion>();
        State = GameOverState.None;
    }

    public void HandleInput(IPlayerConsole player1Console)
    {
        HandlePlayerInput(player1Console, player1);
    }

    public void Handle2PInput(IPlayerConsole player2Console)
    {
        HandlePlayerInput(player2Console, player2);
    }

    private void HandlePlayerInput(IPlayerConsole console, Player player)
    {
        var joystickDirection = console.ReadJoystick();
        var buttons = console.ReadButtons();

        switch (joystickDirection)
        {
            case JoystickDirection.Up:
                player.Move(0, -1);
                break;
            case JoystickDirection.Down:
                player.Move(0, 1);
                break;
            case JoystickDirection.Left:
                player.Move(-1, 0);
                break;
            case JoystickDirection.Right:
                player.Move(1, 0);
                break;
        }

        if (buttons.HasFlag(Buttons.Green))
        {
            PlaceBomb(player.X, player.Y);
        }
    }

    public void Update()
    {
        UpdateBombs();
        UpdateExplosions();
        CheckPlayerCollision();
    }

    private void UpdateBombs()
    {
        for (var i = bombs.Count - 1; i >= 0; i--)
        {
            var bomb = bombs[i];
            bomb.Update();

            if (bomb.IsExploded)
            {
                bombs.RemoveAt(i);
                CreateExplosion(bomb.X, bomb.Y);
            }
        }
    }

    private void UpdateExplosions()
    {
        for (var i = explosions.Count - 1; i >= 0; i--)
        {
            var explosion = explosions[i];
            explosion.Update();

            if (explosion.IsDone)
            {
                explosions.RemoveAt(i);
            }
        }
    }

    private void CheckPlayerCollision()
    {
        foreach (var explosion in explosions)
        {
            var p1died = explosion.CollidesWith(player1.X, player1.Y);
            var p2died = explosion.CollidesWith(player2.X, player2.Y);
            if (p1died && p2died) State = GameOverState.Draw;
            else if (p1died) State = GameOverState.Player2Wins;
            else if (p2died) State = GameOverState.Player1Wins;
            if (p1died || p2died) break;
        }
    }

    public void Draw(IDisplay display)
    {
        grid.Draw(display);
        foreach (var bomb in bombs)
            bomb.Draw(display);
        player1.Draw(display);
        player2.Draw(display);
        foreach (var explosion in explosions)
            explosion.Draw(display);
    }

    public GameOverState State { get; private set; }

    private void PlaceBomb(int x, int y)
    {
        bombs.Add(new Bomb(x, y, BombTimer, bombSprite));
    }

    private void CreateExplosion(int x, int y)
    {
        explosions.Add(new Explosion(x, y, ExplosionDuration));
    }
}