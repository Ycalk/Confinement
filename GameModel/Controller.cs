﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection.Metadata;
using System.Threading;
using System.Threading.Tasks;
using Architecture;
using Architecture.Entities;
using Architecture.Entities.System;
using Confinement.GameModel.PositionsGenerator;
using Confinement.View;
using Confinement.View.Scenes.Cubes.Content;
using Microsoft.Xna.Framework;
using Cube = Architecture.Entities.Cube;


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
                player.MouseButtonPress += OnMouseButtonPress;
                player.MouseButtonRelease += OnMouseButtonRelease;
                player.MouseMove += OnMouseMove;

                player.LeftArrowPressing += OnLeftArrowPress;
                player.RightArrowPressing += OnRightArrowPress;
                player.UpArrowPressing += OnUpArrowPress;
                player.DownArrowPressing += OnDownArrowPress;
                player.EscapePress += OnEscapePress;

                Screen = screen;
                _requests = new Queue<ModelRequest>();
                _exit = exit;
            }

            public static void CreateRequest(ModelRequest request) =>
                _requests.Enqueue(request);

            public void StartModel()
            {
                _currentScene = Scene.Empty(Main.Graphics);
                _field = new Field(25,
                    new MazeScaling(2, .35),
                    new NormalDistribution(Field.FieldElement.DoubleMove, 30),
                    (new SmartEnemy(), new EnemyCube()),
                    (new SmartEnemy(), new EnemyCube()));
                _currentScene = View.Scenes.MainMenu.Scene.GetScene(_field);
                Task.Run(() =>
                {
                    var startField = _field;
                    Thread.Sleep(5000);
                    while (startField == _field)
                    {
                        _field.MoveEnemies();
                        Thread.Sleep(50);
                        var cubes = _currentScene.GetEntities<Cube>().ToArray();
                        foreach (var cube in cubes)
                            _currentScene.Ignore(cube);
                        Thread.Sleep(10000);
                        if (_playState == PlayState.ComputerWin)
                            _playState = PlayState.ComputerMove;
                    }
                });
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

                foreach (var button in _currentScene.GetEntities<Button>().Cast<InterfaceButton>())
                    button.Pause();
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

                foreach (var button in _currentScene.GetEntities<Button>().Cast<InterfaceButton>())
                    button.UnPause();

                _playState = PlayState.PlayerMove;
            }

            public void Exit() =>
                _exit();

            public void Manage(GameTime gameTime)
            {
                _currentScene.Update(gameTime, Screen);
                if (_requests.Count > 0)
                    Execute(_requests.Dequeue());
                if (_requests.Count > 0)
                    _requests.Clear();

                if (_state == GameState.MainMenu)
                {

                    _currentScene.RotateCamera(MathHelper.ToRadians(0.01f));
                    return;
                }

                if (_field is null)
                    throw new InvalidOperationException("Field is not initialized");

                switch (_playState)
                {
                    case PlayState.ComputerWin:
                        ComputerWin();
                        break;
                    case PlayState.PlayerWin:
                        PlayerWin();
                        break;

                    case PlayState.ComputerMove:
                        Task.Run(()=>_field.MoveEnemies());
                        break;

                    case PlayState.GameOver:
                        _currentScene.RotateCamera(MathHelper.ToRadians(.5f));
                        break;

                    case PlayState.PlayerMove:
                    case PlayState.Pause:
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
                    if (_playState != PlayState.PlayerMove || _currentScene.ChangingTarget) return;

                    request.Request.Execute(request.Sender);
                    PlayerMove?.Invoke(move.Pressed.Position);
                    _playState = PlayState.ComputerMove;
                }
                else
                    request.Request.Execute(request.Sender);
            }


            private void PlayerWin()
            {
                var currentLevel = _gameMode.CurrentLevel;
                AddCompletedLevel(currentLevel);
                _playState = PlayState.GameOver;
                _gameMode.OnPlayerWin();
            }

            private void ComputerWin()
            {
                _playState = PlayState.GameOver;
                _gameMode.OnPlayerLose();
            }

            private void UpdateScreen(Vector2 mousePosition) =>
                Screen = new Screen(Main.Graphics.Viewport.Width, Main.Graphics.Viewport.Height, mousePosition);

            private void OnMouseButtonPress(Vector2 position) =>
                _currentScene.ButtonPress();

            private void OnMouseButtonRelease(Vector2 position) =>
                _currentScene.ButtonRelease();

            private void OnMouseMove(Vector2 oldPosition, Vector2 newPosition) =>
                UpdateScreen(newPosition);

            private void OnEscapePress()
            {
                if (_state == GameState.MainMenu)
                    return;

                if (_playState == PlayState.Pause)
                    UnPauseGame();
                else if (_playState == PlayState.PlayerMove)
                    CreateRequest(new ModelRequest(new PauseGame(), null));
            }

            private void OnLeftArrowPress()
            {
                if (_playState != PlayState.Pause && _playState != PlayState.GameOver && _state != GameState.MainMenu)
                    _currentScene.LeftArrowPress();
            }

            private void OnRightArrowPress()
            {
                if (_playState != PlayState.Pause && _playState != PlayState.GameOver && _state != GameState.MainMenu)
                    _currentScene.RightArrowPress();
            }

            private void OnUpArrowPress()
            {
                if (_playState != PlayState.Pause && _playState != PlayState.GameOver && _state != GameState.MainMenu)
                    _currentScene.UpArrowPress();
            }

            private void OnDownArrowPress()
            {
                if (_playState != PlayState.Pause && _playState != PlayState.GameOver && _state != GameState.MainMenu)
                    _currentScene.DownArrowPress();
            }
                
        }
    }
}
