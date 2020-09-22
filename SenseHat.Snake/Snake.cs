using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Numerics;
using System.Threading.Tasks;
using Iot.Device.SenseHat;
using SenseHat.Snake.Joystick;
using SenseHat.Snake.Screen;
using SenseHat.Snake.Utilities.DataStructures;

namespace SenseHat.Snake
{
    public interface ISnake : IDisposable
    {
        Task Run();
    }

    public class Snake : ISnake
    {
        private readonly IJoystick _joystick;
        private readonly ISnakeRenderer _snakeRenderer;
        private readonly int updatesPerSecond = 150;
        private readonly int maxBounds = 8;
        private CircularQueue<Point> snake;
        private Point food;
        private bool ateLastTime;
        private Point headPosition => snake.GetHead();
        private Vector2 direction;

        private bool _isRunning;

        private readonly Random _random;

        public Snake(IJoystick joystick, ISnakeRenderer snakeRenderer)
        {
            _random = new Random();
            _joystick = joystick;
            _snakeRenderer = snakeRenderer;
            _isRunning = false;
            snake = new CircularQueue<Point>();
            snake.EnqueueWithoutDequeue(new Point(3, 3));
            direction = new Vector2(1, 0);
            ateLastTime = false;
            food = GenerateNewFoodPosition();
        }

        private Point GenerateNewFoodPosition()
        {
            Point point;
            do
            {
                point = new Point(_random.Next(maxBounds), _random.Next(maxBounds));
            } while (snake.ToArray().Contains(point));

            return point;
        }


        public async Task Run()
        {
            _isRunning = true;
            _snakeRenderer.Render(snake, food);

            while (_isRunning)
            {
                var joystickState = _joystick.GetNextState();
                // Console.WriteLine(joystickState.ToString());

                if (joystickState.Clicked) Stop();

                var newDirection = direction;
                if (joystickState.Down) newDirection = new Vector2(0, 1);
                if (joystickState.Up) newDirection = new Vector2(0, -1);
                if (joystickState.Left) newDirection = new Vector2(-1, 0);
                if (joystickState.Right) newDirection = new Vector2(1, 0);

                var newHeadPosition = CalculateNewHeadPosition(headPosition, newDirection);
                var secondPosition = snake.Get(1);
                if (newHeadPosition != secondPosition)
                    direction = newDirection;
                else
                    newHeadPosition = CalculateNewHeadPosition(headPosition, direction);

                //Check if not dead?
                if (snake.ToArray().Contains(newHeadPosition))
                {
                    Console.WriteLine("Oh dear!");
                    Console.WriteLine($"But you did get a score of {snake.ToArray().Length}");
                    _isRunning = false;
                    break;
                }

                if (ateLastTime)
                    snake.EnqueueWithoutDequeue(newHeadPosition);
                else
                    snake.Enqueue(newHeadPosition);

                ateLastTime = false;
                if (newHeadPosition == food)
                {
                    ateLastTime = true;
                    food = GenerateNewFoodPosition();
                }

                _snakeRenderer.Render(snake, food);

                await Task.Delay(updatesPerSecond);
            }

            _snakeRenderer.Clear();
        }

        private Point CalculateNewHeadPosition(Point headPosition, Vector2 direction)
        {
            var headAfterNextMove = new Point(
                (maxBounds + headPosition.X + (int) direction.X) % maxBounds,
                (maxBounds + headPosition.Y + (int) direction.Y) % maxBounds
            );

            return headAfterNextMove;
        }

        public void Stop() => _isRunning = false;

        public void Dispose()
        {
            _joystick.Dispose();
        }
    }
}