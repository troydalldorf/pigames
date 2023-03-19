using Core.Display;
using Core.Inputs;
using Pong;

Console.WriteLine("Pong game starting...");

var display = new LedDisplay();
var player1Console = new Player1Console();
var player2Console = new Player2Console();

var pongGame = new PongGame();
pongGame.Run(display, player1Console, player2Console);