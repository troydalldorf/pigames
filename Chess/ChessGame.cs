using System.Drawing;
using Core;
using Core.Display;
using Core.Display.Sprites;
using Core.Effects;
using Core.Inputs;
using Core.State;

namespace Chess;

// TO DO:
// 4. Implement checkmate
// 5. Implement castling
// 6. Implement en passant
// 7. Implement pawn promotion
// 8. Implement stalemate
// 9. Implement draw by repetition
// 10. Implement draw by 50 move rule
// 11. Implement draw by insufficient material
// 13. Implement draw by 5-fold repetition

public class ChessGame : IDuoPlayableGameElement
{
    private Location cursor = new Location(4, 4);
    private Location? selected = null;
    private readonly SpriteAnimation whitePieces;
    private readonly SpriteAnimation blackPieces;
    private readonly ChessBoard board;
    private readonly Explosions explosions;
    private PieceColor currentPlayer = PieceColor.White;
    private DateTimeOffset lastMoveTime = DateTimeOffset.Now;

    public ChessGame()
    {
        var image = SpriteImage.FromResource("chess.png", new Point(1, 1));
        blackPieces = image.GetSpriteAnimation(10, 1, 8, 8, 6, 1);
        whitePieces = image.GetSpriteAnimation(10, 10, 8, 8, 6, 1);
        this.board = new ChessBoard();
        this.board.Reset();
        this.explosions = new Explosions();
        State = GameOverState.None;
    }

    public void Update()
    {
        explosions.Update();
    }

    public void Draw(IDisplay display)
    {
        var cursorColor = currentPlayer == PieceColor.White ? Color.SpringGreen : Color.Magenta;
        for (var i = 0; i < 8; i++)
        {
            for (var j = 0; j < 8; j++)
            {
                var piece = this.board.GetPieceAt(i, j);
                Color color;
                if ((i + j) % 2 == 0)
                {
                    color = Color.Black;
                }
                else
                {
                    color = Color.White;
                }

                if (selected?.X == i && selected?.Y == j)
                {
                    color = cursorColor;
                }

                display.DrawRectangle(i * 8, j * 8, 8, 8, color, color);
                if (piece != null)
                {
                    var sprite = piece?.Color == PieceColor.Black ? blackPieces : whitePieces;
                    sprite.Draw(display, i * 8, j * 8, (int)piece!.Type);
                }
            }
        }

        display.DrawRectangle(cursor.X * 8, cursor.Y * 8, 8, 8, cursorColor);
        explosions.Draw(display);
    }

    public void HandleInput(IPlayerConsole player1Console)
    {
        if (currentPlayer == PieceColor.White)
        {
            HandlePlayerInput(player1Console);
        }
    }

    public void Handle2PInput(IPlayerConsole player2Console)
    {
        if (currentPlayer == PieceColor.Black)
        {
            player2Console = new PlayerConsoleInversionDecorator(player2Console);
            HandlePlayerInput(player2Console);
        }
    }

    public void HandlePlayerInput(IPlayerConsole console)
    {
        // Prevent trigger happy players from moving the cursor too fast
        if (lastMoveTime.AddMilliseconds(200) > DateTimeOffset.Now)
        {
            return;
        }

        var stick = console.ReadJoystick();
        var buttons = console.ReadButtons();
        if (stick != 0 || buttons != 0)
        {
            lastMoveTime = DateTimeOffset.Now;
        }

        if (stick.IsUp()) cursor = cursor.MoveUp();
        if (stick.IsDown()) cursor = cursor.MoveDown();
        if (stick.IsLeft()) cursor = cursor.MoveLeft();
        if (stick.IsRight()) cursor = cursor.MoveRight();

        if (buttons.IsGreenPushed())
        {
            var cursorPiece = board.GetPieceAt(cursor);
            if (selected == null && cursorPiece?.Color == currentPlayer)
            {
                selected = cursor;
            }
            else if (selected != null)
            {
                if (this.board.TryMove(selected, cursor))
                {
                    currentPlayer = currentPlayer == PieceColor.White ? PieceColor.Black : PieceColor.White;
                    explosions.Explode(cursor.X * 8 + 4, cursor.Y * 8 + 4);
                }
                selected = null;
            }
        }
    }

    public GameOverState State { get; }
}