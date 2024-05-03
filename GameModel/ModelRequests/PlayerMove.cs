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
            private readonly Cube _pressed;
            public void Execute()
            {
                if (!_currentScene.GetEntities<Cube>().Any())
                    throw new InvalidOperationException("Scene does not have cubes");

                var playerCube = new Cube(
                    _pressed.World.Translation + new Vector3(0, Content.CubeSizeWithOffset, 0), Content.Cube);

                _currentScene.Add(playerCube);
                _currentScene.Ignore(_pressed);
                _currentScene.Ignore(playerCube);
            }

            public PlayerMove(Cube pressed)
            {
                _pressed = pressed;
            }
        }
    }

}
