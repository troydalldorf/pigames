using Core.Display;
using Core.Inputs;
using ePong;

Console.WriteLine("Pong game starting...");

var display = new LedDisplay();
var player1Console = new Player1Console();
var player2Console = new Player2Console();

var pongGame = new PongGame(display, player1Console, player2Console);
pongGame.Run();