using Core.Display;
using Core.Inputs;
using Tetris;

Console.WriteLine("Tetris game starting...");

var display = new LedDisplay();
var playerConsole = new PlayerConsole(0x3a, 0x42);

var tetrisGame = new SoloTetrisGame();
tetrisGame.Run(display, playerConsole);