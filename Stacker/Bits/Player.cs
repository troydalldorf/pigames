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
        this._y = config.DisplayHeight - 1 - config.BlockSize.Height;
        this._x = 0;
        this._blocks = config.InitialBlocks;
    }

    public bool IsDone { get; private set; }

    public int StackSize => _stack.Count;

    public void Next()
    {
        // Debounce
        if (DateTime.Now < nextInput) return;
        nextInput = DateTime.Now.AddMilliseconds(200);
        var x = _x;
        var blocks = _blocks;
        if (_stack.Any())
        {
            var last = _stack.Last();
            var blockWidth = _config.BlockSize.Width + 1;
            var x1 = Math.Max(last.X, _x) / blockWidth;
            var x2 = Math.Min(last.X + last.Blocks, _x + _config.BlockSize.Width) / blockWidth;
            x = x1 * blockWidth;
            blocks = (x2 - x1);
            IsDone = blocks <= 0;
        }

        if (!IsDone)
        {
            _stack.Add(new StackedBlock(x, blocks));
            _direction *= -1;
            _x = _direction < 0 ? _config.DisplayWidth - 1 - _config.BlockSize.Width * _blocks : 0;
            _y -= _config.BlockSize.Height + 1;
        }
    }

    public void Update()
    {
        _x += _direction;
        var pixelWidth = (_config.BlockSize.Width + 1) * _blocks;
        if (_x + pixelWidth >= _config.DisplayWidth || _x <= 0) _direction *= -1;
    }

    public void Draw(IDisplay display)
    {
        var y = _config.DisplayHeight - 1;
        foreach (var block in _stack)
        {
            DrawRow(display, y, block);
            y -= _config.BlockSize.Height + 1;
        }

        DrawRow(display, _y, new StackedBlock(_x, _blocks));
    }

    private void DrawRow(IDisplay display, int y, StackedBlock block)
    {
        for (var i = 0; i < block.Blocks; i++)
        {
            display.DrawRectangle(
                block.X + (_config.BlockSize.Width + 1) * i,
                y - _config.BlockSize.Height,
                _config.BlockSize.Width,
                _config.BlockSize.Height,
                _config.Color,
                _config.Color);
        }
    }

    private record StackedBlock(int X, int Blocks);
}