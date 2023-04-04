using All;
using Core.Display;
using Core.Display.Fonts;
using Core.Effects;
using Core.Runner;

Console.WriteLine("Game menu starting...");
var display = new LedDisplay();
var fontFactory = new FontFactory();
var runner = new GameRunner(display, fontFactory);
runner.Run(()=>new Menu(runner, fontFactory), canPause:false);
Console.WriteLine("Existing menu...");