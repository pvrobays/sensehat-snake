using System;
using System.Threading.Tasks;
using Iot.Device.SenseHat;

namespace SenseHat.Snake.Joystick
{
    public interface IJoystick : IDisposable
    {
        JoystickState GetNextState();
        void Reset();
    }

    public class Joystick : IJoystick
    {
        private readonly SenseHatJoystick _joyStick;
        private readonly int _refreshRate;

        private bool _isRunning;
        private JoystickState lastState;

        public Joystick(int refreshRate)
        {
            _refreshRate = refreshRate;
            _joyStick = new SenseHatJoystick();
            _isRunning = false;
            lastState = new JoystickState();
            Start();
        }

        private void Start()
        {
            _isRunning = true;
            Task.Run(StartTask);
        }

        private void StartTask()
        {
            while (_isRunning)
            {
                _joyStick.Read();
                if (_joyStick.HoldingDown || _joyStick.HoldingUp || _joyStick.HoldingLeft || _joyStick.HoldingRight)
                {
                    if (_joyStick.HoldingDown && _joyStick.HoldingUp) continue;
                    if (_joyStick.HoldingLeft && _joyStick.HoldingRight) continue;
                    lastState = new JoystickState
                    {
                        Down = _joyStick.HoldingDown,
                        Up = _joyStick.HoldingUp,
                        Left = _joyStick.HoldingLeft,
                        Right = _joyStick.HoldingRight,
                    };
                }
                else if (_joyStick.HoldingButton)
                {
                    lastState = new JoystickState {Clicked = true};
                }

                Task.Delay(_refreshRate).Wait();
            }
        }

        public JoystickState GetNextState()
        {
            var state = lastState;
            Reset();
            return state;
        }

        public void Reset() => lastState = new JoystickState();

        private void Stop() => _isRunning = false;

        public void Dispose()
        {
            Stop();
            _joyStick.Dispose();
        }
    }

    public class JoystickState
    {
        public bool Up { get; set; }
        public bool Down { get; set; }
        public bool Right { get; set; }
        public bool Left { get; set; }

        public bool Clicked { get; set; }

        public JoystickState()
        {
            Up = false;
            Down = false;
            Left = false;
            Right = false;
            Clicked = false;
        }

        public override string ToString()
        {
            string dir = "";
            if (Up) dir += "^";
            if (Down) dir += ".";
            if (Left) dir += "<";
            if (Right) dir += ">";
            
            return $"🕹 {dir}";
        }
    }
}