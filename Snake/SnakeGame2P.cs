using Core;

namespace Snake;

using Core.Inputs;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Threading;
using Core.Display;

public class SnakeGame2P
{
    private enum Direction
    {
        Up,
        Down,
        Left,
        Right
    }

    private const int Width = 64;
    private const int Height = 64;
    private const int SnakeSize = 2;
    private const int InitialSnakeLength = 5;

    private LedDisplay display;
    private PlayerConsole player1Console;
    private PlayerConsole player2Console;

    private List<Point> snake1;
    private Direction snakeDirection1;
    private Point food1;

    private List<Point> snake2;
    private Direction snakeDirection2;
    private Point food2;

    private bool gameRunning;

    public SnakeGame2P(LedDisplay display, PlayerConsole player1Console, PlayerConsole player2Console)
    {
        this.display = display;
        this.player1Console = player1Console;
        this.player2Console = player2Console;

        snake1 = new List<Point>();
        snake2 = new List<Point>();

        gameRunning = false;
    }

    public void Run()
    {
        gameRunning = true;
        InitializeSnake(snake1, Direction.Right);
        InitializeSnake(snake2, Direction.Left);
        SpawnFood(ref food1);
        SpawnFood(ref food2);

        while (gameRunning)
        {
            HandleInput(player1Console, ref snakeDirection1);
            HandleInput(player2Console, ref snakeDirection2);
            UpdateSnake(snake1, snakeDirection1, ref food1);
            UpdateSnake(snake2, snakeDirection2, ref food2);
            CheckCollision(snake1);
            CheckCollision(snake2);
            Draw(snake1, food1, Color.Green, Color.Red);
            Draw(snake2, food2, Color.Blue, Color.Yellow);

            Thread.Sleep(100);
        }
    }

    private void InitializeSnake(List<Point> snake, Direction direction)
    {
        snake.Clear();
        if (direction == Direction.Left)
        {
            for (var i = 0; i < InitialSnakeLength; i++)
            {
                snake.Add(new Point(Width - i * SnakeSize - SnakeSize, Height / 2));
            }
        }
        else
        {
            for (var i = 0; i < InitialSnakeLength; i++)
            {
                snake.Add(new Point(i * SnakeSize, Height / 2));
            }
        }

        snakeDirection1 = direction;
    }

    private void SpawnFood(ref Point food)
    {
        var random = new Random();
        var x = random.Next(0, Width / SnakeSize) * SnakeSize;
        var y = random.Next(0, Height / SnakeSize) * SnakeSize;
        food = new Point(x, y);
    }

    private void HandleInput(PlayerConsole playerConsole, ref Direction snakeDirection)
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

    private void UpdateSnake(List<Point> snake, Direction snakeDirection, ref Point food)
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
            SpawnFood(ref food);
        }
        else
        {
            // Move the snake by removing the tail and adding the new position as the head
            snake.RemoveAt(snake.Count - 1);
            snake.Insert(0, newPosition);
        }
    }

    private void CheckCollision(List<Point> snake)
    {
        // Check collision with the wall
        if (snake[0].X < 0 || snake[0].X >= Width || snake[0].Y < 0 || snake[0].Y >= Height)
        {
            gameRunning = false;
            return;
        }

        // Check collision with itself
        for (var i = 1; i < snake.Count; i++)
        {
            if (snake[0] != snake[i]) continue;
            gameRunning = false;
            return;
        }
    }

    private void Draw(List<Point> snake, Point food, Color snakeColor, Color foodColor)
    {
        display.Clear();

        // Draw snake
        foreach (var point in snake)
        {
            display.DrawRectangle(point.X, point.Y, SnakeSize, SnakeSize, snakeColor);
        }

        // Draw food
        display.DrawRectangle(food.X, food.Y, SnakeSize, SnakeSize, foodColor);

        display.Update();
    }
}