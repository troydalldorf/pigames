using BomberMan.Bits;
using Core;
using Core.Display;
using Core.Display.Sprites;
using Core.Effects;
using Core.Fonts;
using Core.Inputs;
using Core.State;

namespace BomberMan;

using System.Collections.Generic;

public class BombermanGame : IDuoPlayableGameElement
{
    private const int GridSize = 8;
    private const int BombTimer = 3000;

    private Grid grid;
    private Player player1;
    private Player player2;
    private List<Bomb> bombs;
    private List<Bomb> explodedBombs;
    private readonly Explosions explosions = new();
    private readonly Sprite p1Sprite;
    private readonly Sprite p2Sprite;
    private readonly Sprite bombSprite;

    public BombermanGame(IFontFactory fontFactory)
    {
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
        explodedBombs = new List<Bomb>();
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
        explosions.Update();
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
                explodedBombs.Add(bomb);
                this.explosions.Explode(bomb.X*8+4, bomb.Y*8+4, null, () => explodedBombs.Remove(bomb));
            }
        }
    }

    private void CheckPlayerCollision()
    {
        foreach (var bomb in explodedBombs)
        {
            var p1died = bomb.CollidesWith(player1.X, player1.Y);
            var p2died = bomb.CollidesWith(player2.X, player2.Y);
            switch (p1died)
            {
                case true when p2died:
                    State = GameOverState.Draw;
                    break;
                case true:
                    State = GameOverState.Player2Wins;
                    break;
                default:
                {
                    if (p2died) State = GameOverState.Player1Wins;
                    break;
                }
            }
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
        explosions.Draw(display);
    }

    public GameOverState State { get; private set; }

    private void PlaceBomb(int x, int y)
    {
        bombs.Add(new Bomb(x, y, BombTimer, bombSprite));
    }
}