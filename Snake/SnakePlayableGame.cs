using Core;
using Core.Display;

namespace Snake;

using System;
using System.Collections.Generic;
using System.Drawing;

public class SnakePlayableGame : IPlayableGameElement
{
    private enum Direction { Up, Down, Left, Right }

    private const int Width = 64;
    private const int Height = 64;
    private const int SnakeSize = 2;
    private const int InitialSnakeLength = 5;

    private List<Point> snake;
    private Direction snakeDirection;
    private Point food;

    public SnakePlayableGame()
    {
        snake = new List<Point>();
        InitializeSnake();
        SpawnFood();
    }

    private void InitializeSnake()
    {
        snake.Clear();
        snakeDirection = Direction.Right;

        const int centerX = Width / 2;
        const int centerY = Height / 2;

        for (var i = 0; i < InitialSnakeLength; i++)
        {
            snake.Add(new Point(centerX - i * SnakeSize, centerY));
        }
    }

    private void SpawnFood()
    {
        var random = new Random();
        var x = random.Next(0, Width / SnakeSize) * SnakeSize;
        var y = random.Next(0, Height / SnakeSize) * SnakeSize;
        food = new Point(x, y);
    }

    public void HandleInput(IPlayerConsole playerConsole)
    {
        var stick = playerConsole.ReadJoystick();

        if (stick.IsLeft() && snakeDirection != Direction.Right)
            snakeDirection = Direction.Left;
        else if (stick.IsRight() && snakeDirection != Direction.Left)
            snakeDirection = Direction.Right;
        else if (stick.IsUp() && snakeDirection != Direction.Down)
            snakeDirection = Direction.Up;
        else if (stick.IsDown() && snakeDirection != Direction.Up)
            snakeDirection = Direction.Down;
    }

    public void Update()
    {
        var newPosition = new Point(snake[0].X, snake[0].Y);

        switch (snakeDirection)
        {
            case Direction.Up:
                newPosition.Y -= SnakeSize;
                break;
            case Direction.Down:
                newPosition.Y += SnakeSize;
                break;
            case Direction.Left:
                newPosition.X -= SnakeSize;
                break;
            case Direction.Right:
                newPosition.X += SnakeSize;
                break;
        }

        // If the snake eats the food
        if (newPosition == food)
        {
            snake.Insert(0, newPosition);
            SpawnFood();
        }
        else
        {
            // Move the snake by removing the tail and adding the new position as the head
            snake.RemoveAt(snake.Count - 1);
            snake.Insert(0, newPosition);
        }
        
        // Check collision with the wall
        if (snake[0].X < 0 || snake[0].X >= Width || snake[0].Y < 0 || snake[0].Y >= Height)
        {
            State = GameOverState.EndOfGame;
            return;
        }

        // Check collision with itself
        for (var i = 1; i < snake.Count; i++)
        {
            if (snake[0] != snake[i]) continue;
            State = GameOverState.EndOfGame;
            return;
        }
    }

    public void Draw(IDisplay display)
    {
        foreach (var point in snake)
        {
            display.DrawRectangle(point.X, point.Y, SnakeSize, SnakeSize, Color.Green);
        }
        display.DrawRectangle(food.X, food.Y, SnakeSize, SnakeSize, Color.Red);
    }

    public GameOverState State { get; private set; }
}
