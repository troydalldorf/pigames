using Core.Display;
using Core.Inputs;

Console.WriteLine("Pong game starting...");

var display = new LedDisplay();
var player1Console = new Player1Console();

var pongGame = new TowersOfHanoiGame(display, player1Console, 3);
pongGame.Run();