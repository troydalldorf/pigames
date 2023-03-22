using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using Core;

public class ConnectFourGame : IGameElement
{
    private const int BoardWidth = 7;
    private const int BoardHeight = 6;

    private readonly Color[] boardColors = { Color.Black, Color.Red, Color.Yellow };
    private readonly int[,] board = new int[BoardWidth, BoardHeight];

    private int currentPlayer = 1;
    private bool isDone = false;

    public void HandleInput(IPlayerConsole playerConsole)
    {
        // Read the joystick input to determine which column the player wants to drop their piece into.
        var joystick = playerConsole.ReadJoystick();
        if (joystick.HasFlag(JoystickDirection.Left))
        {
            MoveCurrentPiece(-1);
        }
        else if (joystick.HasFlag(JoystickDirection.Right))
        {
            MoveCurrentPiece(1);
        }

        // If the player presses the button, drop the piece into the current column.
        var buttons = playerConsole.ReadButtons();
        if (buttons.HasFlag(Buttons.Red))
        {
            DropCurrentPiece();
        }
    }

    public void Handle2PInput(IPlayerConsole player2Console)
    {
        if (currentPlayer == 2)
        {
            HandleInput(player2Console);
        }
    }

    public void Update()
    {
        // Check if the game is over.
        if (IsGameOver())
        {
            isDone = true;
            return;
        }

        // Switch to the other player's turn.
        currentPlayer = (currentPlayer == 1) ? 2 : 1;
    }

    public void Draw(IDisplay display)
    {
        // Draw the board.
        for (var x = 0; x < BoardWidth; x++)
        {
            for (var y = 0; y < BoardHeight; y++)
            {
                var color = boardColors[board[x, y]];
                display.DrawRectangle(x * 10, y * 10, 10, 10, color, color);
            }
        }

        // Draw the current piece.
        var currentPieceColor = boardColors[currentPlayer];
        var currentPieceX = (currentPieceColumn * 10) + 5;
        var currentPieceY = 5;
        display.DrawCircle(currentPieceX, currentPieceY, 4, currentPieceColor);
    }

    public bool IsDone() => isDone;

    private int currentPieceColumn = BoardWidth / 2;

    private void MoveCurrentPiece(int offset)
    {
        currentPieceColumn = Math.Max(0, Math.Min(BoardWidth - 1, currentPieceColumn + offset));
    }

    private void DropCurrentPiece()
    {
        // Find the lowest empty slot in the current column.
        int row = -1;
        for (int y = BoardHeight - 1; y >= 0; y++)
        {
            if (board[currentPieceColumn, y] == 0)
            {
                row = y;
                break;
            }
        }

        // If the column is full, do nothing.
        if (row == -1)
        {
            return;
        }

        // Drop the piece into the board.
        board[currentPieceColumn, row] = currentPlayer;
    }

    private bool IsGameOver()
    {
        // Check for horizontal wins.
        for (int y = 0; y < BoardHeight; y++)
        {
            for (int x = 0; x < BoardWidth - 3; x++)
            {
                if (board[x, y] == currentPlayer && board[x + 1, y] == currentPlayer &&
                    board[x + 2, y] == currentPlayer && board[x + 3, y] == currentPlayer)
                {
                    return true;
                }
            }
        }

        // Check for vertical wins.
        for (int x = 0; x < BoardWidth; x++)
        {
            for (int y = 0; y < BoardHeight - 3; y++)
            {
                if (board[x, y] == currentPlayer && board[x, y + 1] == currentPlayer &&
                    board[x, y + 2] == currentPlayer && board[x, y + 3] == currentPlayer)
                {
                    return true;
                }
            }
        }

        // Check for diagonal wins (left to right).
        for (int x = 0; x < BoardWidth - 3; x++)
        {
            for (int y = 0; y < BoardHeight - 3; y++)
            {
                if (board[x, y] == currentPlayer && board[x + 1, y + 1] == currentPlayer &&
                    board[x + 2, y + 2] == currentPlayer && board[x + 3, y + 3] == currentPlayer)
                {
                    return true;
                }
            }
        }

        // Check for diagonal wins (right to left).
        for (int x = BoardWidth - 1; x >= 3; x--)
        {
            for (int y = 0; y < BoardHeight - 3; y++)
            {
                if (board[x, y] == currentPlayer && board[x - 1, y + 1] == currentPlayer &&
                    board[x - 2, y + 2] == currentPlayer && board[x - 3, y + 3] == currentPlayer)
                {
                    return true;
                }
            }
        }

        // Check for a tie.
        if (board.Cast<int>().All(x => x != 0))
        {
            return true;
        }

        // No win or tie yet.
        return false;
    }
}