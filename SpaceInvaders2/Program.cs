using System.Reflection;
using Core.Effects;
using SpaceInvaders2;

Console.WriteLine("Space Invaders game starting...");

var assembly = Assembly.GetExecutingAssembly();
foreach (var resourceName in assembly.GetManifestResourceNames())
{
    Console.WriteLine(resourceName);
}
var runner = new GameRunner();
runner.Run(()=>new SpaceInvadersGame());
Console.WriteLine("Exiting Space Invaders...");