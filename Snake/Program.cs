using Core.Display;
using Core.Inputs;
using Snake;

Console.WriteLine("Snake game starting...");

var display = new LedDisplay();
var player1Console = new Player1Console();
var player2Console = new Player2Console();
//var snakeGame = new SnakeGame2P(display, player1Console, player2Console);
var snakeGame = new SnakeGame(display, player1Console);
snakeGame.Run();