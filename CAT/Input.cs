using Microsoft.Xna.Framework.Input;

namespace CAT
{
    //Class to handle detecting if buttons were just pressed vs held
    static class Input
    {
        private static KeyboardState _previousState;
        private static KeyboardState _currentState;
        private static MouseState _previousMouse;
        private static MouseState _currentMouse;

        public static bool KeyHeld(Keys key)
        {
            return _currentState.IsKeyDown(key);
        }

        public static bool KeyPressed(Keys key)
        {
            return _currentState.IsKeyDown(key) && !_previousState.IsKeyDown(key);
        }

        public static bool KeyReleased(Keys key)
        {
            return !_currentState.IsKeyDown(key) && _previousState.IsKeyDown(key);
        }
        public static bool LeftClick()
        {
            return _currentMouse.LeftButton == ButtonState.Pressed && _previousMouse.LeftButton == ButtonState.Released;
        }

        public static bool RightClick()
        {
            return _currentMouse.RightButton == ButtonState.Pressed && _previousMouse.RightButton == ButtonState.Released;
        }

        public static bool MWheelUp()
        {
            return _currentMouse.ScrollWheelValue > _previousMouse.ScrollWheelValue;
        }

        public static bool MWheelDown()
        {
            return _currentMouse.ScrollWheelValue < _previousMouse.ScrollWheelValue;
        }

        public static void Update()
        {
            _previousState = _currentState;
            _currentState = Keyboard.GetState();
            _previousMouse = _currentMouse;
            _currentMouse = Mouse.GetState();
        }
    }
}
