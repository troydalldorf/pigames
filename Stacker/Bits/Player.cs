using System.Drawing;
using Core;
using Core.Display;

namespace Stacker.Bits;

public class Player : IGameElement
{
    private List<StackedBlock> _stack = new();
    private readonly Config _config;
    private int _direction = 1;
    private int _x;
    private int _y;
    private int _blocks;
    private int _speed = 1;
    private DateTime nextInput = DateTime.Now;

    public Player(Config config)
    {
        this._config = config;
        this._y = config.DisplayHeight - 1;
        this._x = 0;
        this._blocks = config.InitialBlocks;
    }

    public bool IsDone { get; private set; }

    public int Score => _stack.Sum(x => x.Blocks);
    
    public int StackSize => _stack.Count;

    public void Next()
    {
        // Debounce
        if (DateTime.Now < nextInput) return;
        nextInput = DateTime.Now.AddMilliseconds(200);

        var currentColumn = SnapToCol(_x, _direction);
        var currentBlocks = _blocks;
    
        if (_stack.Any())
        {
            var last = _stack.Last();
            var left = Math.Max(currentColumn, last.Column);
            var right = Math.Min(currentColumn + currentBlocks, last.Blocks);
            currentColumn = left;
            currentBlocks = right - left;
        }

        // If no blocks captured, the game is done
        if (currentBlocks == 0)
        {
            IsDone = true;
            return;
        }

        Console.WriteLine($"Snap col: {currentColumn}");
        // Update stack and change direction
        _stack.Add(new StackedBlock(currentColumn, currentBlocks));
        _direction *= -1;
        _blocks = currentBlocks; // The number of blocks for the next moving set is updated here
        _x = _direction < 0 ? _config.DisplayWidth - 1 - _config.BlockSize.Width * currentBlocks : 0;
        _y -= _config.BlockSize.Height + 1;
    }

    private int SnapToCol(int x, int direction = -1) =>
        x / (_config.BlockSize.Width + 1) + direction == 1 ? _config.BlockSize.Width + 1 : 0; 
    
    public void Update()
    {
        _x += _direction;
        var pixelWidth = (_config.BlockSize.Width + 1) * _blocks;
        if (_direction < 0 && _x <= 0) _direction *= -1;
        if (_direction > 0 && _x + pixelWidth >= _config.DisplayWidth) _direction *= -1;
    }

    public void Draw(IDisplay display)
    {
        var y = _config.DisplayHeight - 1;
        foreach (var block in _stack)
        {
            DrawRow(display, y, block);
            y -= _config.BlockSize.Height + 1;
        }

        DrawRow(display, _y, _x, _blocks);
    }

    private void DrawRow(IDisplay display, int y, StackedBlock block)
    {
        var x = block.Column * (_config.BlockSize.Width + 1);
        DrawRow(display, y, x, block.Blocks);
    }

    private void DrawRow(IDisplay display, int y, int x, int blocks)
    {
        for (var i = 0; i < blocks; i++)
        {
            display.DrawRectangle(
                x + (_config.BlockSize.Width + 1) * i,
                y - _config.BlockSize.Height,
                _config.BlockSize.Width,
                _config.BlockSize.Height,
                _config.Color,
                _config.Color);
        }
    }

    private record StackedBlock(int Column, int Blocks);
}