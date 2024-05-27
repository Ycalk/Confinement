

using System;
using System.Diagnostics;
using Architecture;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Confinement
{
    internal class Player
    {
        public event Action<Vector2> MouseButtonPress;
        public event Action<Vector2> MouseButtonRelease;
        public event Action LeftArrowPressing;
        public event Action RightArrowPressing;
        public event Action UpArrowPressing;
        public event Action DownArrowPressing;
        public event Action<Vector2, Vector2> MouseMove;
        public event Action<Screen, Screen> WindowResize;

        private MouseState _mouseState;
        private Screen _screen;
        public void ProcessControls(int windowWidth, int windowHeight)
        {
            var currentState = Mouse.GetState();
            var previousState = _mouseState;
            var screen = new Screen(windowWidth, windowHeight, currentState.X, currentState.Y);


            if (currentState.LeftButton == ButtonState.Pressed 
                && previousState.LeftButton == ButtonState.Released)
                OnMouseButtonPress(new Vector2(currentState.X, currentState.Y));

            else if (currentState.LeftButton == ButtonState.Released && previousState.LeftButton == ButtonState.Pressed)
                OnMouseButtonRelease(new Vector2(currentState.X, currentState.Y));


            if (currentState.X != previousState.X || currentState.Y != previousState.Y)
                OnMouseMove(new Vector2(previousState.X, previousState.Y),
                    new Vector2(currentState.X, currentState.Y));

            if (screen.Width != _screen.Width && screen.Height != _screen.Height)
                OnWindowResize(_screen, screen);

            var keyboard = Keyboard.GetState();

            if (keyboard.IsKeyDown(Keys.Left) || keyboard.IsKeyDown(Keys.A))
                LeftArrowPressing?.Invoke();

            if (keyboard.IsKeyDown(Keys.Right) || keyboard.IsKeyDown(Keys.D))
                RightArrowPressing?.Invoke();

            if (keyboard.IsKeyDown(Keys.Up) || keyboard.IsKeyDown(Keys.W))
                UpArrowPressing?.Invoke();

            if (keyboard.IsKeyDown(Keys.Down) || keyboard.IsKeyDown(Keys.S))
                DownArrowPressing?.Invoke();
            
            _screen = screen;
            _mouseState = currentState;
        }

        private void OnWindowResize(Screen oldScreen, Screen newScreen) =>
            WindowResize?.Invoke(oldScreen, newScreen);

        private void OnMouseButtonRelease(Vector2 position) =>
            MouseButtonRelease?.Invoke(position);

        private void OnMouseButtonPress(Vector2 position) =>
            MouseButtonPress?.Invoke(position);

        private void OnMouseMove(Vector2 previousPosition, Vector2 newPosition) =>
            MouseMove?.Invoke(previousPosition, newPosition);
    }
}
