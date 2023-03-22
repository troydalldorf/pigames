using Core.Effects;

Console.WriteLine("Connect Four game starting...");
var runner = new GameRunner();
runner.Run(() => new ConnectFourGame());