﻿using Core.Effects;
using ePong;

Console.WriteLine("Pong game starting...");
var runner = new GameRunner();
runner.Run(()=>new EPongPlayableGame(), 50);
