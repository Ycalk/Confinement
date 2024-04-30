using System;
using System.Collections;
using System.Collections.Generic;
using Architecture;
using Architecture.Entities;
using Confinement.View;
using Confinement.View.System;
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
            public Controller(Player.Player player, Screen screen)
            {
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
                ParseRequests();
                // some logic if it is playing
            }

            private void ParseRequests()
            {
                if (_requests.Count > 0)
                    _requests.Dequeue().Request.Invoke(this);
            }

            public void LoadScene(Scene scene)
            {
                if ((scene is ISceneWithCubes && _currentScene is not ISceneWithCubes)
                    || (_currentScene is ISceneWithCubes && scene is not ISceneWithCubes
                                                         && GameState == State.Playing
                                                         && _playStay is PlayStay.PlayerWin or PlayStay.ComputerWin))
                {
                    _currentScene = scene;
                    SceneChange?.Invoke(_currentScene);
                }
                else
                {
                    throw new InvalidOperationException("Cannot change scene");
                }
            }

            public void PlayerMove(Cube pressed)
            {
                if (_currentScene is not ISceneWithCubes scene)
                    throw new InvalidOperationException("Scene does not have cubes");

                var playerCube = new Cube(
                    pressed.World.Translation + new Vector3(0, Content.CubeSizeWithOffset, 0), Content.Cube);

                scene.AddCube(playerCube);
                scene.IgnoreCube(pressed);
                scene.IgnoreCube(playerCube);
            }
        }
        
    }
}
