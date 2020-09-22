using System;
using System.Drawing;
using SenseHat.Snake.Utilities.DataStructures;

namespace SenseHat.Snake.Screen
{
    public interface ISnakeRenderer
    {
        void Render(CircularQueue<Point> snake, Point food);
        void Clear();
    }

    public class SnakeRenderer : ISnakeRenderer
    {
        private readonly IEfficientLedMatrix _efficientLedMatrix;
        private int width => _efficientLedMatrix.GetMaxWidth();
        private int height => _efficientLedMatrix.GetMaxHeight();

        public SnakeRenderer(IEfficientLedMatrix efficientLedMatrix)
        {
            _efficientLedMatrix = efficientLedMatrix;
        }


        public void Render(CircularQueue<Point> snake, Point food)
        {
            var canvas = CreateBlankCanvas(Color.Black);

            var headColor = Color.Aqua;
            // var headColor = RandomColor(); // for some extra fun
            var foodColor = Color.Firebrick;
            
            canvas[food.Y * width + food.X] = foodColor;
            
            foreach (var item in snake.ToArray())
            {
                canvas[item.Y * width + item.X] = headColor;
            }

            _efficientLedMatrix.Render(canvas);
        }

        public void Clear() => _efficientLedMatrix.Render(CreateBlankCanvas(Color.Black));

        private Color[] CreateBlankCanvas(Color color)
        {
            var length = width * height;
            var canvas = new Color[length];
            for (var i = 0; i < length; i++)
                canvas[i] = color;

            return canvas;
        }

        private Random _rand = new Random();
        private Color RandomColor()
        {
            return Color.FromArgb(
                _rand.Next(255),
                _rand.Next(255),
                _rand.Next(255)
            );
        }
    }
}