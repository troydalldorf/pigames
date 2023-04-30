using System.Drawing;
using Core;
using Core.Display.Sprites;

namespace Blitz;

public class BlitzGame : IPlayableGameElement
{
    private const int DisplayHeight = 64;
    private const int DisplayWidth = 64;
    private const int Buildings = 8;
    private const int MinBuildingHeight = 2;
    private const int MaxBuildingHeight = 7;
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
    private ISprite plane;
    private ISprite bomb;
    private ISprite building;
    private ISprite grass;

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

        buildingHeights = new int[8];
        var random = new Random();

        for (var i = 0; i < Buildings ; i++)
        {
            buildingHeights[i] = random.Next(MinBuildingHeight, MaxBuildingHeight);
        }
    }

    public void HandleInput(IPlayerConsole playerConsole)
    {
        var buttons = playerConsole.ReadButtons();

        if (!bombDropped && buttons.HasFlag(Buttons.Green))
        {
            bombDropped = true;
            bombX = (planeX+plane.Width/2) / 8;
            bombY = planeY;
        }
    }

    public void Update()
    {
        if (gameState != GameOverState.None)
        {
            return;
        }

        planeX++;

        if (planeX >= DisplayWidth)
        {
            planeX = plane.Width;
            planeY += PlaneYSpeed;
        }

        if (bombDropped)
        {
            bombY++;

            if (bombY < buildingHeights[bombX])
            {
                buildingHeights[bombX]--;
                bombDropped = false;
                score++;
            }
        }

        if (planeY < buildingHeights[planeX])
        {
            gameState = GameOverState.EndOfGame;
        }
        else if (planeY == 0)
        {
            gameState = GameOverState.Player1Wins;
        }
    }

    public void Draw(IDisplay display)
    {
        // Draw buildings
        for (int i = 0; i < Buildings; i++)
        {
            for (int y = 0; i < buildingHeights[i]; y++)
            {
                building.Draw(display, i * 8, DisplayHeight - y * 8);
            }
        }

        // Draw plane
        plane.Draw(display, planeX, planeY);

        // Draw bomb
        if (bombDropped)
        {
            bomb.Draw(display, bombX * 8, bombY * 8);
        }
    }

    public GameOverState State => gameState;
}