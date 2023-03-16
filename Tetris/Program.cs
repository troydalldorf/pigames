// See https://aka.ms/new-console-template for more information

using Core.Display;
using Core.Inputs;
using Tetris;

Console.WriteLine("Tetris game starting...");

var display = new LedDisplay();
var playerConsole = new PlayerConsole(0x3a, 0x42);

var tetrisGame = new TetrisGame(display, playerConsole);
tetrisGame.Run();