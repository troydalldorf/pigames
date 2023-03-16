using Core.Display;
using Core.Inputs;
using Snake;

Console.WriteLine("Snake game starting...");

var display = new LedDisplay();
var playerConsole = new PlayerConsole(0x3a, 0x42);
var snakeGame = new SnakeGame(display, playerConsole);
snakeGame.Run();