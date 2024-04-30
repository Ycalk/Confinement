

using System;
using Architecture;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Confinement.Player
{
    internal class Player
    {
        public event Action<Vector2> MouseButtonPress;
        public event Action<Vector2> MouseButtonRelease;
        public event Action LeftArrowPressing;
        public event Action RightArrowPressing;
        public event Action<Vector2, Vector2> MouseMove;
        public event Action<Screen, Screen> WindowResize;

        private MouseState _mouseState;
        private Screen _screen;
        public void ProcessControls(int windowWidth, int windowHeight)
        {
            var currentState = Mouse.GetState();
            var previousState = _mouseState;
            var screen = new Screen(windowWidth, windowHeight, currentState.X, currentState.Y);

            switch (currentState.LeftButton)
            {
                case ButtonState.Pressed when previousState.LeftButton == ButtonState.Released:
                    OnMouseButtonPress(new Vector2(currentState.X, currentState.Y));
                    break;
                case ButtonState.Released when previousState.LeftButton == ButtonState.Pressed:
                    OnMouseButtonRelease(new Vector2(currentState.X, currentState.Y));
                    break;
            }

            if (currentState.X != previousState.X || currentState.Y != previousState.Y)
                OnMouseMove(new Vector2(previousState.X, previousState.Y), 
                    new Vector2(currentState.X, currentState.Y));

            if (screen.Width != _screen.Width && screen.Height != _screen.Height)
                OnWindowResize(_screen, screen);
            
            var keyboard = Keyboard.GetState();

            if (keyboard.IsKeyDown(Keys.Left))
                LeftArrowPressing?.Invoke();

            if (keyboard.IsKeyDown(Keys.Right))
                RightArrowPressing?.Invoke();

            _screen = screen;
            _mouseState = currentState;
        }

        private void OnWindowResize(Screen oldScreen, Screen newScreen) =>
            WindowResize?.Invoke(oldScreen, newScreen);

        private void OnMouseButtonRelease(Vector2 position) =>
            MouseButtonRelease?.Invoke(position);

        private void OnMouseButtonPress (Vector2 position) =>
            MouseButtonPress?.Invoke(position);

        private void OnMouseMove(Vector2 previousPosition, Vector2 newPosition) =>
            MouseMove?.Invoke(previousPosition, newPosition);
    }
}
