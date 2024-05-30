using Confinement.GameModel.PositionsGenerator;
using Confinement.View;
using Confinement.View.Scenes.Cubes.Content;
using Microsoft.Xna.Framework;
using System.Diagnostics;
using System;
using Cube = Architecture.Entities.Cube;
using Architecture.Entities.System;

namespace Confinement.GameModel
{
    internal partial class GameModel
    {
        internal class StartGame : IGamePlot
        {
            private readonly int _enemyCount;
            private readonly int _fieldSize;
            private readonly int _scalingCoefficient;
            private readonly double _scalingChance;
            private readonly int _doubleMovePointsCount;
            

            public void Execute(Entity sender)
            {
                if (new StackTrace().GetFrame(1)!.GetMethod()!.DeclaringType != typeof(Controller))
                    throw new InvalidOperationException("Method can only be execute in controller");

                if (_state == GameState.MainMenu)
                {
                    var enemies = new (IEnemyAlgorithm, Cube)[_enemyCount];
                    for (var i = 0; i < _enemyCount; i++)
                        enemies[i] = (new SmartEnemy(), new EnemyCube());

                    _state = GameState.Playing;
                    _field = new Field(_fieldSize,
                        new MazeScaling(_scalingCoefficient, _scalingChance),
                        new NormalDistribution(Field.FieldElement.DoubleMove, _doubleMovePointsCount),
                        enemies);
                    _playState = PlayState.PlayerMove;
                    _controller.LoadScene(View.Scenes.Cubes.Scene.GetScene(_field, 70));
                }
            }

            public StartGame(int enemyCount, int fieldSize) :
                this(enemyCount, fieldSize, 2, .25, fieldSize * 3)
            {
            }

            public StartGame(int enemyCount, int fieldSize, int scalingCoefficient, double scalingChance, int doubleMovePointsCount)
            {
                _enemyCount = enemyCount;
                _fieldSize = fieldSize;
                _scalingCoefficient = scalingCoefficient;
                _scalingChance = scalingChance;
                _doubleMovePointsCount = doubleMovePointsCount;
            }
        }
    }
}
