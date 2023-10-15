using System.Drawing;
using Core;
using Core.Display;

namespace Stacker;

public class StackerGame2P : IDuoPlayableGameElement
{
    private const int DisplayWidth = 64;
    private const int DisplayHeight = 64;
    private const int BlockWidth = 4;
    private const int MaxBlockCount = DisplayHeight / BlockWidth;

    private int currentY1;
    private int currentX1;
    private int lastX1;
    private int direction1;
    private int blockCount1;

    private int currentY2;
    private int currentX2;
    private int lastX2;
    private int direction2;
    private int blockCount2;

    private int speed;

    public StackerGame2P()
    {
        Reset();
        State = GameOverState.None;
    }

    private void Reset()
    {
        currentY1 = DisplayHeight - BlockWidth;
        currentY2 = 0;
        currentX1 = DisplayWidth / 4 - BlockWidth;
        currentX2 = 3 * DisplayWidth / 4 - BlockWidth;
        direction1 = direction2 = 1;
        blockCount1 = blockCount2 = MaxBlockCount;
        speed = 1;
    }

    public void HandleInput(IPlayerConsole player1Console)
    {
        var buttons = player1Console.ReadButtons();
        if (buttons != Buttons.None)
        {
            if (currentX1 != lastX1)
            {
                State = GameOverState.Player2Wins;
                return;
            }

            lastX1 = currentX1;
            currentY1 -= BlockWidth;
            blockCount1--;

            if (blockCount1 == 0)
            {
                State = GameOverState.Player1Wins;
            }
        }
    }

    public void Handle2PInput(IPlayerConsole player2Console)
    {
        var buttons = player2Console.ReadButtons();
        if (buttons != Buttons.None)
        {
            if (currentX2 != lastX2)
            {
                State = GameOverState.Player1Wins;
                return;
            }

            lastX2 = currentX2;
            currentY2 += BlockWidth;
            blockCount2--;

            if (blockCount2 == 0)
            {
                State = GameOverState.Player2Wins;
            }
        }
    }

    public void Update()
    {
        currentX1 += direction1 * speed;
        currentX2 += direction2 * speed;

        // Check for boundaries and bounce back for both players
        if (currentX1 <= 0 || currentX1 >= DisplayWidth / 2 - BlockWidth)
        {
            direction1 *= -1;
        }

        if (currentX2 <= DisplayWidth / 2 || currentX2 >= DisplayWidth - BlockWidth)
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