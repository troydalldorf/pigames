using Core;
using Core.Display;
using Core.Inputs;

namespace TicTacToe;
using System.Drawing;
using System.Threading;

internal class TicTacToeGame
{
    private const int BoardSize = 3;
    private const int CellSize = 16;
    private const int CellPadding = 2;
    private const int BoardPadding = 10;
    private const int TurnDelay = 100;

    private LedDisplay display;
    private PlayerConsole player1Console;
    private PlayerConsole player2Console;
    private Cell[,] board;
    private bool isPlayer1Turn;

    public TicTacToeGame(LedDisplay display, PlayerConsole player1Console, PlayerConsole player2Console)
    {
        this.display = display;
        this.player1Console = player1Console;
        this.player2Console = player2Console;

        board = new Cell[BoardSize, BoardSize];

        for (var row = 0; row < BoardSize; row++)
        {
            for (var col = 0; col < BoardSize; col++)
            {
                board[row, col] = new Cell();
            }
        }

        isPlayer1Turn = true;
    }

    public void Run()
    {
        while (true)
        {
            DrawBoard();
            HandleInput();
        }
    }

    private void DrawBoard()
    {
        display.Clear();

        for (var row = 0; row < BoardSize; row++)
        {
            for (var col = 0; col < BoardSize; col++)
            {
                var x = col * (CellSize + CellPadding) + BoardPadding;
                var y = row * (CellSize + CellPadding) + BoardPadding;

                if (board[row, col].Value == CellValue.Player1)
                {
                    display.DrawRectangle(x, y, CellSize, CellSize, Color.White);
                    display.DrawLine(x, y, x + CellSize, y + CellSize, Color.Red);
                    display.DrawLine(x, y + CellSize, x + CellSize, y, Color.Red);
                }
                else if (board[row, col].Value == CellValue.Player2)
                {
                    display.DrawRectangle(x, y, CellSize, CellSize, Color.White);
                    display.DrawCircle(x + CellSize / 2, y + CellSize / 2, CellSize / 2, Color.Blue);
                }
                else
                {
                    display.DrawRectangle(x, y, CellSize, CellSize, Color.Black);
                }
            }
        }
        

        display.Update();
    }

    private void HandleInput()
    {
        var stick = isPlayer1Turn ? player1Console.ReadJoystick() : player2Console.ReadJoystick();
        var buttons = isPlayer1Turn ? player1Console.ReadButtons() : player2Console.ReadButtons();

        if (stick.IsUp())
        {
            MoveCursor(-1, 0);
            Thread.Sleep(TurnDelay);
        }
        else if (stick.IsDown())
        {
            MoveCursor(1, 0);
            Thread.Sleep(TurnDelay);
        }
        else if (stick.IsLeft())
        {
            MoveCursor(0, -1);
            Thread.Sleep(TurnDelay);
        }
        else if (stick.IsRight())
        {
            MoveCursor(0, 1);
            Thread.Sleep(TurnDelay);
        }
        else if (buttons > 0)
        {
            if (board[selectedRow, selectedCol].Value != CellValue.Empty) return;
            board[selectedRow, selectedCol].Value = isPlayer1Turn ? CellValue.Player1 : CellValue.Player2;
            isPlayer1Turn = !isPlayer1Turn;
            if (!CheckForWinner() && !CheckForDraw()) return;
            Thread.Sleep(2000);
            ResetGame();
        }
    }

    private void MoveCursor(int deltaRow, int deltaCol)
    {
        var newRow = selectedRow + deltaRow;
        var newCol = selectedCol + deltaCol;

        if (newRow is < 0 or >= BoardSize || newCol is < 0 or >= BoardSize) return;
        selectedRow = newRow;
        selectedCol = newCol;
    }

    private bool CheckForWinner()
    {
        // Check rows
        for (int row = 0; row < BoardSize; row++)
        {
            if (board[row, 0].Value != CellValue.Empty &&
                board[row, 0].Value == board[row, 1].Value &&
                board[row, 1].Value == board[row, 2].Value)
            {
                HighlightWinner(row, 0, row, 1, row, 2);
                return true;
            }
        }

        // Check columns
        for (int col = 0; col < BoardSize; col++)
        {
            if (board[0, col].Value != CellValue.Empty &&
                board[0, col].Value == board[1, col].Value &&
                board[1, col].Value == board[2, col].Value)
            {
                HighlightWinner(0, col, 1, col, 2, col);
                return true;
            }
        }

        // Check diagonals
        if (board[0, 0].Value != CellValue.Empty &&
            board[0, 0].Value == board[1, 1].Value &&
            board[1, 1].Value == board[2, 2].Value)
        {
            HighlightWinner(0, 0, 1, 1, 2, 2);
            return true;
        }

        if (board[0, 2].Value != CellValue.Empty &&
            board[0, 2].Value == board[1, 1].Value &&
            board[1, 1].Value == board[2, 0].Value)
        {
            HighlightWinner(0, 2, 1, 1, 2, 0);
            return true;
        }

        return false;
    }

    private bool CheckForDraw()
    {
        for (int row = 0; row < BoardSize; row++)
        {
            for (int col = 0; col < BoardSize; col++)
            {
                if (board[row, col].Value == CellValue.Empty)
                {
                    return false;
                }
            }
        }

        return true;
    }

    private void HighlightWinner(int row1, int col1, int row2, int col2, int row3, int col3)
    {
        int x1 = col1 * (CellSize + CellPadding) + BoardPadding;
        int y1 = row1 * (CellSize + CellPadding) + BoardPadding;
        int x2 = col2 * (CellSize + CellPadding) + BoardPadding;
        int y2 = row2 * (CellSize + CellPadding) + BoardPadding;
        int x3 = col3 * (CellSize + CellPadding) + BoardPadding;
        int y3 = row3 * (CellSize + CellPadding) + BoardPadding;

        display.DrawLine(x1 + CellSize / 2, y1 + CellSize / 2, x2 + CellSize / 2, y2 + CellSize / 2, Color.Yellow
        );
        display.DrawLine(x2 + CellSize / 2, y2 + CellSize / 2, x3 + CellSize / 2, y3 + CellSize / 2, Color.Yellow);
        display.DrawLine(x3 + CellSize / 2, y3 + CellSize / 2, x1 + CellSize / 2, y1 + CellSize / 2, Color.Yellow);
        display.Update();
    }

    private void ResetGame()
    {
        for (int row = 0; row < BoardSize; row++)
        {
            for (int col = 0; col < BoardSize; col++)
            {
                board[row, col].Value = CellValue.Empty;
            }
        }

        isPlayer1Turn = true;
    }

    private int selectedRow = 0;
    private int selectedCol = 0;

    private class Cell
    {
        public CellValue Value { get; set; }
    }

    private enum CellValue
    {
        Empty,
        Player1,
        Player2
    }
}