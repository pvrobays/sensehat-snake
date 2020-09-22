using System;
using System.Drawing;
using Iot.Device.SenseHat;

namespace SenseHat.Snake.Screen
{
    public interface IEfficientLedMatrix : IDisposable
    {
        void Render(Color[] colorMatrix);
        int GetMaxWidth();
        int GetMaxHeight();
    }
    
    public class EfficientLedMatrix : IEfficientLedMatrix
    {
        private readonly int width = 8;
        private readonly int height = 8;
        private readonly SenseHatLedMatrix _ledMatrix;

        private Color[] previousColorMatrix;

        public EfficientLedMatrix()
        {
            _ledMatrix = new SenseHatLedMatrixI2c();
            previousColorMatrix = new Color[width * height];
            for (var i = 0; i < width * height; i++)
                previousColorMatrix[i] = Color.Black;
        }

        public void Render(Color[] colorMatrix)
        {
            if (colorMatrix.Length != height * width)
                throw new ArgumentException($"LEDs should be of length {height * width} but is {colorMatrix.Length}", nameof(colorMatrix));

            for (var i = 0; i < width * height; i++)
            {
                if (previousColorMatrix[i] == colorMatrix[i]) continue;

                var x = i % width;
                var y = i / width;
                
                _ledMatrix.SetPixel(x, y, colorMatrix[i]);
            }

            previousColorMatrix = colorMatrix;
        }

        public int GetMaxWidth() => width;

        public int GetMaxHeight() => height;

        public void Dispose()
        {
            _ledMatrix.Dispose();
        }
    }
}