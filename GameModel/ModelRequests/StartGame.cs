using Confinement.GameModel.PositionsGenerator;
using Confinement.View;
using Confinement.View.Scenes.Cubes.Content;
using Microsoft.Xna.Framework;
using System.Diagnostics;
using System;
using System.Threading;
using System.Threading.Tasks;
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
            private readonly int _level;

            public void Execute(Entity sender)
            {
                if (new StackTrace().GetFrame(1)!.GetMethod()!.DeclaringType != typeof(Controller))
                    throw new InvalidOperationException("Method can only be execute in controller");

                if (_additionalAction is not null)
                    _additionalAction(sender);

                var enemies = new (IEnemyAlgorithm, Cube)[_enemyCount];
                for (var i = 0; i < _enemyCount; i++)
                    enemies[i] = (new SmartEnemy(), new EnemyCube());

                _currentScene.Reset();

                if (_selectedGameMode is not null)
                    _gameMode = _selectedGameMode;
                _state = GameState.Playing;
                _field = new Field(_fieldSize,
                    new MazeScaling(_scalingCoefficient, _scalingChance),
                    new NormalDistribution(Field.FieldElement.DoubleMove, _doubleMovePointsCount),
                    enemies);
                _playState = PlayState.PlayerMove;
                _controller.LoadScene(View.Scenes.Cubes.Scene.GetScene(_field, 70, _level));
                Task.Run(() =>
                {
                    var counter = 0;
                    while (counter != 50)
                    {
                        _currentScene.RightArrowPress();
                        _currentScene.UpArrowPress();
                        Thread.Sleep(10);
                        counter++;
                    }

                    Thread.Sleep(1000);

                    if (CompletedLevels.Count == 0)
                    {
                        Controller.CreateRequest(new ModelRequest(new ShowInstruction(), null));
                    }
                });
            }

            public StartGame(int level, IGameMode gameMode = null, Action<Entity> additionalAction = null)
            {
                _level = level;
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
                        _fieldSize = 19;
                        _scalingCoefficient = 2;
                        _scalingChance = .25;
                        _doubleMovePointsCount = 20;
                        break;
                    case 3:
                        _enemyCount = 3;
                        _fieldSize = 27;
                        _scalingCoefficient = 2;
                        _scalingChance = .25;
                        _doubleMovePointsCount = 50;
                        break;
                    case 4:
                        _enemyCount = 4;
                        _fieldSize = 39;
                        _scalingCoefficient = 2;
                        _scalingChance = .20;
                        _doubleMovePointsCount = 90;
                        break;
                    case 5:
                        _enemyCount = 5;
                        _fieldSize = 47;
                        _scalingCoefficient = 2;
                        _scalingChance = .19;
                        _doubleMovePointsCount = 150;
                        break;
                    default:
                        throw new ArgumentOutOfRangeException(nameof(level));
                }
            }
        }
    }
}
