using Core.Effects;

Console.WriteLine("Frogger game starting...");
var runner = new GameRunner();
runner.Run(() => new MemoryCardGame());