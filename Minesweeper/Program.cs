using Core.Display;
using Core.Inputs;
using Minesweeper;

Console.WriteLine("Minesweeper game starting...");

var display = new LedDisplay();
var playerConsole = new PlayerConsole(0x3a, 0x42);
var snakeGame = new MinesweeperGame(display, playerConsole);
snakeGame.Run();