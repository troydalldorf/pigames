using System;
using System.Collections.Generic;
using System.Drawing;
using System.Threading;
using Core;
using Core.Display.Fonts;

public class TowersOfHanoiGame
{
    private IDisplay _display;
    private IPlayerConsole _playerConsole;
    private LedFont _font;

    private int _numDisks;
    private List<Stack<int>> _pegs;
    private int _selectedPeg;

    public TowersOfHanoiGame(IDisplay display, IPlayerConsole playerConsole, int numDisks = 3)
    {
        _display = display;
        _playerConsole = playerConsole;
        _font = new LedFont(LedFontType.FontTomThumb);
        _numDisks = numDisks;

        InitializeGameState();
    }

    private void InitializeGameState()
    {
        _pegs = new List<Stack<int>>()
        {
            new Stack<int>(),
            new Stack<int>(),
            new Stack<int>()
        };

        for (int i = _numDisks; i > 0; i--)
        {
            _pegs[0].Push(i);
        }

        _selectedPeg = -1;
    }

    public void Run()
    {
        while (true)
        {
            _display.Clear();
            UpdatePlayerInput();
            DrawElements();
            _display.Update();
            Thread.Sleep(100);
        }
    }

    private void UpdatePlayerInput()
    {
        var buttons = _playerConsole.ReadButtons();
        var joystick = _playerConsole.ReadJoystick();

        if (joystick.HasFlag(JoystickDirection.Left) || joystick.HasFlag(JoystickDirection.Right))
        {
            int direction = joystick.HasFlag(JoystickDirection.Left) ? -1 : 1;
            _selectedPeg = (_selectedPeg + direction + 3) % 3;
        }

        if (buttons.HasFlag(Buttons.Green))
        {
            if (_pegs[_selectedPeg].Count > 0)
            {
                int disk = _pegs[_selectedPeg].Pop();
                int targetPeg = (_selectedPeg + 1) % 3;
                if (_pegs[targetPeg].Count == 0 || _pegs[targetPeg].Peek() > disk)
                {
                    _pegs[targetPeg].Push(disk);
                }
                else
                {
                    _pegs[_selectedPeg].Push(disk);
                }
            }
        }
    }

    private void DrawElements()
    {
        int pegWidth = _display.Width / 3;
        int diskHeight = _display.Height / (_numDisks + 2);

        for (int pegIndex = 0; pegIndex < _pegs.Count; pegIndex++)
        {
            Stack<int> peg = _pegs[pegIndex];
            int pegX = pegWidth * pegIndex + pegWidth / 2;
            int pegY = _display.Height - diskHeight;

            for (int diskIndex = 0; diskIndex < peg.Count; diskIndex++)
            {
                int disk = peg.ToArray()[diskIndex];
                int diskWidth = pegWidth * disk / (_numDisks + 1);
                int diskX = pegX - diskWidth / 2;
                int diskY = pegY - diskIndex * diskHeight;
                _display.DrawRectangle(diskX, diskY, diskWidth, diskHeight, Color.Black, disk == _numDisks ? Color.Blue : Color.Red);
            }

            // Draw peg
            _display.DrawLine(pegX, _display.Height, pegX, pegY - diskHeight, Color.Black);

            // Draw selection indicator
            if (_selectedPeg == pegIndex)
            {
                _display.DrawRectangle(pegX - pegWidth / 4, _display.Height - 2 * diskHeight, pegWidth / 2, 2, Color.Yellow, Color.Yellow);
            }
        }

        // Draw the score
        _font.DrawText(_display, 5, 5, Color.White, $"Disks: {_numDisks}");
    }
}