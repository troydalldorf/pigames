using Core.Display;
using Core.Inputs;
using SpaceInvaders2;

Console.WriteLine("Space Invaders game starting...");

var display = new LedDisplay();
var playerConsole = new PlayerConsole(0x3a, 0x42);
var snakeGame = new SpaceInvadersGame(display, playerConsole);
snakeGame.Run();