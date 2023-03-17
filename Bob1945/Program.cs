using _1945;
using Core.Display;
using Core.Inputs;

Console.WriteLine("Breakout game starting...");

var display = new LedDisplay();
var player1Console = new Player1Console();
var player2Console = new Player2Console();
var snakeGame = new Game(display, player1Console, player2Console);
snakeGame.Run();