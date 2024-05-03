using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Architecture;
using Architecture.Entities;
using Confinement.View;
using Microsoft.Xna.Framework;


namespace Confinement.GameModel
{
    internal partial class GameModel 
    {
        internal class Controller
        {
            private static Queue<ModelRequest> _requests;
            public static void CreateRequest(ModelRequest request) =>
                _requests.Enqueue(request);

            public event Action<Scene> SceneChange;
            public Controller(Player player, Screen screen)
            {
                _controller = this;
                player.WindowResize += OnWindowResize;
                player.MouseButtonPress += OnMouseButtonPress;
                player.MouseButtonRelease += OnMouseButtonRelease;
                player.MouseMove += OnMouseMove;
                player.LeftArrowPressing += OnLeftArrowPressing;
                player.RightArrowPressing += OnRightArrowPressing;
                Screen = screen;
                _requests = new Queue<ModelRequest>();
            }

            public void StartModel()
            {
                _currentScene = View.Scenes.MainMenu.Scene.GetScene();
                SceneChange?.Invoke(_currentScene);
            }

            private void OnWindowResize(Screen oldScreen, Screen newScreen) =>
                Screen = newScreen;

            private void OnMouseButtonPress(Vector2 position) =>
                _currentScene.ButtonPress();

            private void OnMouseButtonRelease(Vector2 position) =>
                _currentScene.ButtonRelease();

            private void OnMouseMove(Vector2 oldPosition, Vector2 newPosition) =>
                Screen = new Screen(Screen.Width, Screen.Height, newPosition);

            private void OnLeftArrowPressing() =>
                _currentScene.LeftArrowPress();

            private void OnRightArrowPressing() =>
                _currentScene.RightArrowPress();

            public void Manage(GameTime gameTime)
            {
                _currentScene.Update(gameTime, Screen);
                if (_requests.Count > 0)
                    _requests.Dequeue().Request.Execute();
                // some logic if it is playing
            }

            public void LoadScene(Scene scene)
            {
                _currentScene = scene;
                SceneChange?.Invoke(_currentScene);
            }
        }
    }
}
