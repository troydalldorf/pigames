using System.Reflection;
using Core.Effects;
using SpaceInvaders2;

Console.WriteLine("Space Invaders game starting...");
var runner = new GameRunner();
runner.Run(()=>new SpaceInvadersPlayableGame());
Console.WriteLine("Exiting Space Invaders...");