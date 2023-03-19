using Core.Display;
using Core.Inputs;
using Tetris;

Console.WriteLine("Tetris game starting...");

var display = new LedDisplay();
var p1Console = new Player1Console();
var p2Console = new Player2Console();

var tetrisGame = new DuoTetrisGame();
tetrisGame.Run(display, p1Console, p2Console);