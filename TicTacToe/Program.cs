using Core.Display;
using Core.Inputs;
using TicTacToe;

Console.WriteLine("Space Invaders game starting...");

var display = new LedDisplay();
var player1Console = new Player1Console();
var player2Console = new Player2Console();
var snakeGame = new TicTacToeGame(display, player1Console, player2Console);
snakeGame.Run();