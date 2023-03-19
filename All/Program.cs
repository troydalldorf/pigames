using All;
using Core.Display;
using Core.Inputs;

Console.WriteLine("Game menu starting...");

var display = new LedDisplay();
var player1Console = new Player1Console();
var player2Console = new Player2Console();

var menu = new Menu();
menu.Run(display, player1Console, player2Console);