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
            public event Action<Scene> SceneChange;
            public event Action<Vector3> PlayerMove;

            private static Queue<ModelRequest> _requests;
            public static void CreateRequest(ModelRequest request) =>
                _requests.Enqueue(request);

            private Action _exit;
            public Controller(Player player, Screen screen, Action exit)
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
                _exit = exit;
            }

            public void StartModel()
            {
                _currentScene = View.Scenes.MainMenu.Scene.GetScene();
                SceneChange?.Invoke(_currentScene);
            }

            public void Exit() =>
                _exit();

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
                    Execute(_requests.Dequeue());

                if (_state == GameState.Playing)
                {
                    if (_field is null)
                        throw new InvalidOperationException("Field is not initialized");

                    switch (_playState)
                    {
                        case PlayState.ComputerMove:
                            _field.MoveEnemies();
                            break;
                        case PlayState.ComputerWin:
                            ComputerWin();
                            break;
                        case PlayState.PlayerWin:
                            PlayerWin();
                            break;
                        case PlayState.PlayerMove:
                            break;
                    }
                }
            }

            private void Execute(ModelRequest request)
            {
                if (request is null)
                    throw new ArgumentNullException(nameof(request));
                if (request.Request is PlayerMove move)
                {
                    PlayerMove?.Invoke(move.Pressed.World.Translation);
                    _playState = PlayState.ComputerMove;
                }
                request.Request.Execute();
            }


            private void PlayerWin()
            {

            }

            private void ComputerWin()
            {

            }

            public void LoadScene(Scene scene)
            {
                _currentScene = scene;
                SceneChange?.Invoke(_currentScene);
            }
        }
    }
}
