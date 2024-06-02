using Confinement.GameModel.PositionsGenerator;
using Confinement.View;
using Confinement.View.Scenes.Cubes.Content;
using Microsoft.Xna.Framework;
using System.Diagnostics;
using System;
using Cube = Architecture.Entities.Cube;
using Architecture.Entities.System;
using Confinement.GameModel.GameModes;

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
            private readonly Action<Entity> _additionalAction;
            private readonly IGameMode _selectedGameMode;


            public void Execute(Entity sender)
            {
                if (new StackTrace().GetFrame(1)!.GetMethod()!.DeclaringType != typeof(Controller))
                    throw new InvalidOperationException("Method can only be execute in controller");

                if (_additionalAction is not null)
                    _additionalAction(sender);

                var enemies = new (IEnemyAlgorithm, Cube)[_enemyCount];
                for (var i = 0; i < _enemyCount; i++)
                    enemies[i] = (new SmartEnemy(), new EnemyCube());

                _currentScene.Clear();

                if (_selectedGameMode is not null)
                    _gameMode = _selectedGameMode;
                _state = GameState.Playing;
                _field = new Field(_fieldSize,
                    new MazeScaling(_scalingCoefficient, _scalingChance),
                    new NormalDistribution(Field.FieldElement.DoubleMove, _doubleMovePointsCount),
                    enemies);
                _playState = PlayState.PlayerMove;
                _controller.LoadScene(View.Scenes.Cubes.Scene.GetScene(_field, 70));
                
            }

            public StartGame(int level, IGameMode gameMode = null, Action<Entity> additionalAction = null)
            {
                if (additionalAction is not null) 
                    _additionalAction = additionalAction;

                if (gameMode is not null)
                    _selectedGameMode = gameMode;

                if (gameMode is null && _gameMode is null)
                    throw new ArgumentNullException(nameof(IGameMode), "Expected game mode, but was null");

                switch (level)
                {
                    case 1:
                        _enemyCount = 1;
                        _fieldSize = 15;
                        _scalingCoefficient = 2;
                        _scalingChance = .30;
                        _doubleMovePointsCount = 5;
                        break;
                    case 2:
                        _enemyCount = 2;
                        _fieldSize = 17;
                        _scalingCoefficient = 2;
                        _scalingChance = .25;
                        _doubleMovePointsCount = 20;
                        break;
                    case 3:
                        _enemyCount = 3;
                        _fieldSize = 27;
                        _scalingCoefficient = 2;
                        _scalingChance = .23;
                        _doubleMovePointsCount = 50;
                        break;
                    case 4:
                        _enemyCount = 4;
                        _fieldSize = 35;
                        _scalingCoefficient = 2;
                        _scalingChance = .18;
                        _doubleMovePointsCount = 90;
                        break;
                    case 5:
                        _enemyCount = 5;
                        _fieldSize = 45;
                        _scalingCoefficient = 2;
                        _scalingChance = .17;
                        _doubleMovePointsCount = 200;
                        break;
                    case 6:
                        _enemyCount = 6;
                        _fieldSize = 57;
                        _scalingCoefficient = 2;
                        _scalingChance = .17;
                        _doubleMovePointsCount = 300;
                        break;
                    default:
                        throw new ArgumentOutOfRangeException(nameof(level));
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
