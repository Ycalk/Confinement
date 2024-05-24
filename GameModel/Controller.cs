using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Architecture;
using Architecture.Entities;
using Architecture.Entities.System;
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
            private readonly Action _exit;

            private IEnumerable<IInteractive> _ignoringBeforePause;
            private List<Entity> _pause;

            public Controller(Player player, Screen screen, Action exit)
            {
                _controller = this;
                player.WindowResize += OnWindowResize;
                player.MouseButtonPress += OnMouseButtonPress;
                player.MouseButtonRelease += OnMouseButtonRelease;
                player.MouseMove += OnMouseMove;

                player.LeftArrowPressing += OnLeftArrowPress;
                player.RightArrowPressing += OnRightArrowPress;
                player.UpArrowPressing += OnUpArrowPress;
                player.DownArrowPressing += OnDownArrowPress;

                Screen = screen;
                _requests = new Queue<ModelRequest>();
                _exit = exit;
            }

            public static void CreateRequest(ModelRequest request) =>
                _requests.Enqueue(request);

            public void StartModel()
            {
                _currentScene = View.Scenes.MainMenu.Scene.GetScene();
                SceneChange?.Invoke(_currentScene);
            }

            public void PauseGame(List<Entity> pause)
            {
                _playState = PlayState.Pause;
                _pause = pause;
                _ignoringBeforePause = _currentScene.Ignoring.ToArray();
                foreach (var entity in _currentScene.GetEntities<Cube>()
                             .Cast<IInteractive>()
                             .Concat(_currentScene.GetEntities<Button>()))
                    _currentScene.Ignore(entity);
                foreach (var entity in pause)
                    _currentScene.Add(entity);
            }

            public void UnPauseGame()
            {
                foreach (var entity in _currentScene.GetEntities<Cube>()
                             .Cast<IInteractive>()
                             .Concat(_currentScene.GetEntities<Button>())
                             .Except(_ignoringBeforePause))
                    _currentScene.DisableIgnore(entity);
                foreach (var entity in _pause)
                    _currentScene.Remove(entity);

                _playState = PlayState.PlayerMove;
            }

            public void Exit() =>
                _exit();

            public void Manage(GameTime gameTime)
            {
                _currentScene.Update(gameTime, Screen);
                if (_requests.Count > 0)
                    Execute(_requests.Dequeue());

                if (_state != GameState.Playing) return;

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

                    case PlayState.Pause:
                    case PlayState.PlayerMove:
                        break;
                }
            }

            public void LoadScene(Scene scene)
            {
                _currentScene = scene;
                SceneChange?.Invoke(_currentScene);
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

            private void OnWindowResize(Screen oldScreen, Screen newScreen) =>
                Screen = newScreen;

            private void OnMouseButtonPress(Vector2 position) =>
                _currentScene.ButtonPress();

            private void OnMouseButtonRelease(Vector2 position) =>
                _currentScene.ButtonRelease();

            private void OnMouseMove(Vector2 oldPosition, Vector2 newPosition) =>
                Screen = new Screen(Screen.Width, Screen.Height, newPosition);

            private void OnLeftArrowPress()
            {
                if (_playState != PlayState.Pause)
                    _currentScene.LeftArrowPress();
            }

            private void OnRightArrowPress()
            {
                if (_playState != PlayState.Pause)
                    _currentScene.RightArrowPress();
            }

            private void OnUpArrowPress()
            {
                if (_playState != PlayState.Pause)
                    _currentScene.UpArrowPress();
            }

            private void OnDownArrowPress()
            {
                if (_playState != PlayState.Pause)
                    _currentScene.DownArrowPress();
            }
                
        }
    }
}
