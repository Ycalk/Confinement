using System;
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
                if (!_currentScene.GetEntities<Cube>().Any())
                    throw new InvalidOperationException("Scene does not have cubes");

                var playerCube = new Cube(
                    Pressed.World.Translation + new Vector3(0, Content.CubeSizeWithOffset, 0), Content.ObstacleCube);

                _currentScene.Add(playerCube);
                _currentScene.Ignore(Pressed);
                _currentScene.Ignore(playerCube);
            }

            public PlayerMove(Cube pressed)
            {
                Pressed = pressed;
            }
        }
    }

}
