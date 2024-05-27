using System;
using System.Diagnostics;
using System.Linq;
using Architecture.Entities;
using Confinement.View;
using Microsoft.Xna.Framework;

namespace Confinement.GameModel
{
    internal partial class GameModel
    {
        internal class PlayerMove : IGamePlot
        {
            public Cube Pressed { get; }
            public void Execute()
            {
                if (new StackTrace().GetFrame(1)!.GetMethod()!.DeclaringType != typeof(Controller))
                    throw new InvalidOperationException("Method can only be execute in controller");
                if (!_currentScene.GetEntities<Cube>().Any())
                    throw new InvalidOperationException("Scene does not have cubes");

                var playerCube = new View.Scenes.Cubes.Content.Cube(
                    Pressed.World.Translation, Content.DarkGreyTexture);

                _currentScene.Add(playerCube);
                _currentScene.Ignore(Pressed);
                _currentScene.Ignore(playerCube);
                playerCube.MoveTo(Pressed.World.Translation + new Vector3(0, Content.CubeSizeWithOffset, 0), 0.13f, false);
            }

            public PlayerMove(Cube pressed)
            {
                Pressed = pressed;
            }
        }
    }

}
