using System.Drawing;
using Core;
using Core.Display;

namespace Stacker;

public class StackerGame2P : IDuoPlayableGameElement
{
    private const int DisplayWidth = 64;
    private const int DisplayHeight = 64;
    private const int BlockWidth = 4;
    private const int MaxBlockCount = 16;

    private int currentY1;
    private int currentX1;
    private int direction1;
    private int blockCount1;

    private int currentY2;
    private int currentX2;
    private int direction2;
    private int blockCount2;

    private int speed1;
    private int speed2;

    public StackerGame2P()
    {
        Reset();
        State = GameOverState.None;
    }

    private void Reset()
    {
        currentY1 = DisplayHeight - BlockWidth;
        currentY2 = BlockWidth;
        currentX1 = DisplayWidth / 4 - BlockWidth;
        currentX2 = 3 * DisplayWidth / 4 - BlockWidth;
        direction1 = 1;
        direction2 = -1;
        blockCount1 = blockCount2 = MaxBlockCount;
        speed1 = speed2 = 1;
    }

    public void HandleInput(IPlayerConsole player1Console)
    {
        var buttons = player1Console.ReadButtons();
        if ((buttons & Buttons.Green) != 0)
        {
            direction1 *= -1;
            blockCount1--;
            speed1++;
            if (blockCount1 == 0)
            {
                State = GameOverState.Player1Wins;
            }
        }
    }

    public void Handle2PInput(IPlayerConsole player2Console)
    {
        var buttons = player2Console.ReadButtons();
        if ((buttons & Buttons.Blue) == 0) return;
        direction2 *= -1;
        blockCount2--;
        speed2++;
        if (blockCount2 == 0)
        {
            State = GameOverState.Player2Wins;
        }
    }

    public void Update()
    {
        currentX1 += direction1 * speed1;
        currentX2 += direction2 * speed2;

        // Check for boundaries and bounce back
        if (currentX1 is <= 0 or >= DisplayWidth / 2 - BlockWidth)
        {
            direction1 *= -1;
        }

        if (currentX2 is <= DisplayWidth / 2 or >= DisplayWidth - BlockWidth)
        {
            direction2 *= -1;
        }
    }

    public void Draw(IDisplay display)
    {
        display.Clear();

        // Draw blocks for player 1 and player 2
        display.DrawRectangle(currentX1, currentY1, BlockWidth, BlockWidth, Color.Green);
        display.DrawRectangle(currentX2, currentY2, BlockWidth, BlockWidth, Color.Blue);
    }

    public GameOverState State { get; private set; }
}