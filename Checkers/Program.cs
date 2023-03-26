using Core.Effects;

Console.WriteLine("Checkers game starting...");
var runner = new GameRunner();
runner.Run(() => new CheckersPlayableGame());