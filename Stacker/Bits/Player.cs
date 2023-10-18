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

    public int StackSize => _stack.Count;

    public void Next()
    {
        // Debounce
        if (DateTime.Now < nextInput) return;
        nextInput = DateTime.Now.AddMilliseconds(200);

        var x = _x;
        var blocks = _blocks;
        var blockWidth = _config.BlockSize.Width + 1;
    
        if (_stack.Any())
        {
            var last = _stack.Last();
        
            // Calculate overlapping region between the last block and the current block
            int x1, x2;
            if (_direction < 0) // moving left
            {
                x1 = _x;
                x2 = last.X + last.Blocks * blockWidth;
            }
            else // moving right
            {
                x1 = last.X;
                x2 = _x + blockWidth * _blocks;
            }

            if (x2 <= x1) // No overlap
            {
                IsDone = true;
                return;
            }

            x = x1;
            blocks = (x2 - x1) / blockWidth;
        }

        // If no blocks captured, the game is done
        if (blocks == 0)
        {
            IsDone = true;
            return;
        }

        // Update stack and change direction
        _stack.Add(new StackedBlock(x, blocks));
        _direction *= -1;
        _blocks = blocks; // The number of blocks for the next moving set is updated here
        _x = _direction < 0 ? _config.DisplayWidth - 1 - _config.BlockSize.Width * blocks : 0;
        _y -= _config.BlockSize.Height + 1;
    }
    
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