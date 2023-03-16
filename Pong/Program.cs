using Core.Display;
using Core.Inputs;
using Pong;

Console.WriteLine("Pong game starting...");

var display = new LedDisplay();
var player1Console = new PlayerConsole(0x3a, 0x42);
var player2Console = new PlayerConsole(0x43, 0x44);

var pongGame = new PongGame(display, player1Console, player2Console);
pongGame.Run();