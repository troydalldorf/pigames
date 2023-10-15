using System.Drawing;
using Core;
using Core.Display;

namespace Stacker.Bits;

public class Player : IGameElement
{
    private List<StackedBlock> _stack = new();
    private readonly Size _blockSize;
    private readonly Color _color;
    private readonly int _displayWidth;
    private readonly int _displayHeight;
    private int _direction = 1;
    private int _x;
    private int _y;
    private int _speed = 1;

    public Player(Size blockSize, Color color, int displayWidth, int displayHeight)
    {
        this._y = _displayHeight - _blockSize.Height - 1;
        this._x = 0;
        this._blockSize = blockSize;
        this._color = color;
        this._displayWidth = displayWidth;
        this._displayHeight = displayHeight;
    }
    
    public bool IsDone { get; private set; }

    public int StackSize => _stack.Count;

    public void Next()
    {
        var x = _x;
        var width = _blockSize.Width;
        if (_stack.Any())
        {
            var last = _stack.Last();
            var x1 = Math.Max(last.X, _x);
            var x2 = Math.Min(last.X + last.Width, _x + _blockSize.Width);
            width = x2 - x1;
            IsDone = width <= 0;
        }

        if (!IsDone)
        {
            _stack.Add(new StackedBlock(x, width));
            _y -= _blockSize.Height;
        }
    }

    public void Update()
    {
        _x += _direction;
        if (_x+_blockSize.Width >= _displayWidth || _x <= 0) _direction *= -1;
    }

    public void Draw(IDisplay display)
    {
        display.DrawRectangle(_x, _y, _blockSize.Width, _blockSize.Height, _color, _color);
        var y = _displayHeight - _blockSize.Height - 1;
        foreach (var block in _stack)
        {
            display.DrawRectangle(block.X, y - _blockSize.Height, block.Width, _blockSize.Height, _color, _color);
            y -= _blockSize.Height;
        }
    }

    private record StackedBlock(int X, int Width);
}