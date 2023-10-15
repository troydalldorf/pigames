using System.Drawing;
using Core;
using Core.Display;
using Core.Fonts;

namespace TicTacToe
{
    public class TicTacToeGame : IDuoPlayableGameElement
    {
        private const int BoardSize = 3;
        private const int CellSize = 10;
        private const int Padding = 0;
        private const int Offset = 2;
        private const int PieceRadius = 3;

        private readonly IFont font;

        private int[,] board;
        private bool isPlayer1Turn;
        private int cursorX;
        private int cursorY;
        private int winner = 0;

        public GameOverState State { get; private set; } = GameOverState.None;

        public TicTacToeGame(IFontFactory fontFactory)
        {
            font = fontFactory.GetFont(LedFontType.Font4x6);
            InitializeBoard();
        }

        private void InitializeBoard()
        {
            board = new int[BoardSize, BoardSize];
            isPlayer1Turn = true;
        }

        public void HandleInput(IPlayerConsole player1Console)
        {
            if (!isPlayer1Turn) return;
            HandlePlayerInput(player1Console, 1);
        }

        public void Handle2PInput(IPlayerConsole player2Console)
        {
            if (isPlayer1Turn) return;
            HandlePlayerInput(player2Console, 2);
        }

        private void HandlePlayerInput(IPlayerConsole playerConsole, int player)
        {
            var direction = playerConsole.ReadJoystick();
            var buttons = playerConsole.ReadButtons();

            int dx = 0, dy = 0;
            switch (direction)
            {
                case JoystickDirection.Up:
                    dy = -1;
                    break;
                case JoystickDirection.Down:
                    dy = 1;
                    break;
                case JoystickDirection.Left:
                    dx = -1;
                    break;
                case JoystickDirection.Right:
                    dx = 1;
                    break;
            }

            var newX = cursorX + dx;
            var newY = cursorY + dy;

            if (newX >= 0 && newX < BoardSize && newY >= 0 && newY < BoardSize)
            {
                cursorX = newX;
                cursorY = newY;
            }

            if (buttons == Buttons.Green && board[cursorX, cursorY] == 0)
            {
                board[cursorX, cursorY] = player;
                if (IsGameOver())
                {
                    this.State = isPlayer1Turn ? GameOverState.Player1Wins : GameOverState.Player2Wins;
                }
                else
                {
                    isPlayer1Turn = !isPlayer1Turn;
                }
            }
        }

        private bool IsGameOver()
        {
            // Check rows and columns
            for (int i = 0; i < BoardSize; i++)
            {
                if ((board[i, 0] != 0 && board[i, 0] == board[i, 1] && board[i, 1] == board[i, 2]) ||
                    (board[0, i] != 0 && board[0, i] == board[1, i] && board[1, i] == board[2, i]))
                {
                    return true;
                }
            }

            // Check diagonals
            if ((board[0, 0] != 0 && board[0, 0] == board[1, 1] && board[1, 1] == board[2, 2]) ||
                (board[2, 0] != 0 && board[2, 0] == board[1, 1] && board[1, 1] == board[0, 2]))
            {
                return true;
            }
            
            return false;
        }

        public void Update()
        {
            // Add game logic/movement here
        }

        public void Draw(IDisplay display)
        {
            var cursorColor = isPlayer1Turn ? Color.Red : Color.Blue;
            for (int y = 0; y < BoardSize; y++)
            {
                for (int x = 0; x < BoardSize; x++)
                {
                    var xPos = x * CellSize + Padding;
                    var yPos = y * CellSize + Padding;

                    display.DrawRectangle(xPos, yPos, CellSize, CellSize, Color.Gray);

                    // Draw cursor
                    if (x == cursorX && y == cursorY)
                    {
                        display.DrawRectangle(cursorX * CellSize, cursorY * CellSize, CellSize, CellSize, cursorColor);
                    }

                    // Draw pieces
                    switch (board[x, y])
                    {
                        case 1: // Player 1
                            display.DrawLine(xPos + Offset, yPos + Offset, xPos + CellSize - Offset, yPos + CellSize - Offset, Color.Red);
                            display.DrawLine(xPos + CellSize - Offset, yPos + Offset, xPos + Offset, yPos + CellSize - Offset, Color.Red);
                            break;
                        case 2: // Player 2
                            display.DrawCircle(xPos + CellSize / 2, yPos + CellSize / 2, PieceRadius, Color.Blue);
                            break;
                    }
                }
            }
        }
    }
}