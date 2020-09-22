using System;
using System.Threading.Tasks;
using SenseHat.Snake.Screen;

namespace SenseHat.Snake
{
    class Program
    {
        static async Task Main(string[] args)
        {
            Console.WriteLine("Welcome to the snake game!");

            var joystick = new Joystick.Joystick(50);
            var snakeRenderer = new SnakeRenderer(new EfficientLedMatrix());
            var snake = new Snake(joystick, snakeRenderer);
            
            await snake.Run();
            
            Console.WriteLine("Thank you for playing!");
        }
    }
}