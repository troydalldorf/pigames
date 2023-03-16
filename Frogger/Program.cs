using Core.Display;
using Core.Inputs;
using Frogger;

Console.WriteLine("Frogger game starting...");

var display = new LedDisplay();
var playerConsole = new PlayerConsole(0x3a, 0x42);
var snakeGame = new FroggerGame(display, playerConsole);
snakeGame.Run();