using Core.Display;
using Core.Inputs;
using Pacman;

Console.WriteLine("Minesweeper game starting...");

var display = new LedDisplay();
var playerConsole = new PlayerConsole(0x3a, 0x42);
var snakeGame = new PacManGame(display, playerConsole);
snakeGame.Run();