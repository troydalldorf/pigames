// See https://aka.ms/new-console-template for more information

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using CheckMatrix;
using Core.Display;

Console.WriteLine("Hello, World!");

const int maxHeight = 16;
const int colorStep = 15;
const int frameStep = 1;

var matrix = new RgbLedMatrix(new RgbLedMatrixOptions
{
    ChainLength = 1,
    Rows = 64,
    Cols = 64,
});
var canvas = matrix.CreateOffscreenCanvas();
var rnd = new Random();
var points = new List<Point>();
var recycled = new Stack<Point>();
int frame = 0;
var stopwatch = new Stopwatch();

while (!Console.KeyAvailable)
{
    stopwatch.Restart();

    frame++;

    if (frame % frameStep == 0)
    {
        if (recycled.Count == 0)
            points.Add(new Point(rnd.Next(0, canvas.Width - 1), 0));
        else
        {
            var point = recycled.Pop();
            point.x = rnd.Next(0, canvas.Width - 1);
            point.y = 0;
            point.recycled = false;
        }
    }

    canvas.Clear();

    foreach (var point in points)
    {
        if (!point.recycled)
        {
            point.y++;

            if (point.y - maxHeight > canvas.Height)
            {
                point.recycled = true;
                recycled.Push(point);
            }

            for (var i = 0; i < maxHeight; i++)
            {
                canvas.SetPixel(point.x, point.y - i, new Color(0, 255 - i * colorStep, 0));
            }
        }
    }

    canvas = matrix.SwapOnVsync(canvas);

    // force 30 FPS
    var elapsed = stopwatch.ElapsedMilliseconds;
    if (elapsed < 33)
    {
        Thread.Sleep(33 - (int)elapsed);
    }
}

return 0;