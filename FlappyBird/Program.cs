using Core.Display;
using Core.Inputs;
using FlappyBird;

Console.WriteLine("Breakout game starting...");

var display = new LedDisplay();
var playerConsole = new PlayerConsole(0x3a, 0x42);
var snakeGame = new FlappyBirdGame(display, playerConsole);
snakeGame.Run();