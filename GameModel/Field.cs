#nullable enable
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Architecture.Entities;
using Architecture.Entities.System;
using Confinement.GameModel.PositionsGenerator;
using Confinement.View;
using Microsoft.Xna.Framework;
using static Confinement.GameModel.GameModel.Field;

namespace Confinement.GameModel
{
    internal partial class GameModel
    {
        internal class Field
        {
            public class Enemy
            {
                public Point Position { get; set; }
                public IEnemyAlgorithm Algorithm { get; set; }

                public Cube Cube { get; set; }

                public bool IsAlive { get; set; }

                public Enemy(Point position, IEnemyAlgorithm algorithm, Cube cube)
                {
                    Position = position;
                    Algorithm = algorithm;
                    Cube = cube;
                    IsAlive = true;
                }
            }

            private bool _isMoving;

            public enum FieldElement
            {
                Empty,
                Void,
                Enemy,
                Obstacle,
                DoubleMove
            }

            public FieldElement this[int x, int y] => _fieldElements[x, y];

            public int Size => _fieldElements.GetLength(0);

            private readonly FieldElement[,] _fieldElements;
            private readonly List<Enemy> _enemies = new();
            public ReadOnlyCollection<Enemy> Enemies => _enemies.AsReadOnly();

            public Field(int size, IPositionsGenerator obstaclePositions, IPositionsGenerator doubleMovePositions,
                params (IEnemyAlgorithm algorithm, Cube cube)[] enemies)
            {
                _fieldElements = new FieldElement[size, size];
                _controller.PlayerMove += OnPlayerMove;
                switch (enemies.Length)
                {
                    case 0:
                        throw new ArgumentException("At least one enemy must be provided");
                    case > 5:
                        throw new ArgumentException("Too many enemies");
                }

                if (size % 2 == 0)
                    throw new ArgumentException("Size must be odd");

                MakeField();
                AddElements(obstaclePositions, FieldElement.Obstacle);
                AddElements(doubleMovePositions, FieldElement.DoubleMove);
                MakeFieldCircled();
                SpawnEnemies(enemies);
            }

            public void MoveEnemies()
            {
                if (_isMoving)
                    return;
                _isMoving = true;
                var cubes = _currentScene.GetEntities<Cube>().ToArray();
                foreach (var enemy in _enemies)
                    MoveEnemy(enemy, cubes);

                if (_enemies.All(enemy => !enemy.IsAlive))
                    _playState = PlayState.PlayerWin;

                else if (_playState != PlayState.ComputerWin)
                    _playState = PlayState.PlayerMove;
                _isMoving = false;
            }

            private void OnPlayerMove(Vector3 newCube)
            {
                var convertedPosition = ConvertIntoFieldCoordinates(newCube);
                _fieldElements[convertedPosition.X, convertedPosition.Y] = FieldElement.Obstacle;
            }

            private void MoveEnemy(Enemy enemy, Cube[] cubes)
            {
                if (!enemy.IsAlive)
                    return;

                var enemyStartPosition = enemy.Position;
                var enemyNewPosition = enemy.Algorithm.GetMove(this, enemyStartPosition);

                if (enemyStartPosition == enemyNewPosition)
                {
                    enemy.IsAlive = false;
                    enemy.Cube.Disappear(1f);
                    return;
                }

                if (_fieldElements[enemyNewPosition.X, enemyNewPosition.Y] is FieldElement.Obstacle
                    or FieldElement.Enemy)
                    return;

                if (_fieldElements[enemyNewPosition.X, enemyNewPosition.Y] == FieldElement.Void)
                    _playState = PlayState.ComputerWin;

                _fieldElements[enemyNewPosition.X, enemyNewPosition.Y] = FieldElement.Enemy;
                _fieldElements[enemyStartPosition.X, enemyStartPosition.Y] = FieldElement.Empty;

                var underEnemy = GetCube(enemyStartPosition, cubes);
                var newUnderEnemy = GetCube(enemyNewPosition, cubes);

                if (underEnemy is not null)
                    _currentScene.DisableIgnore(underEnemy);
                
                if (newUnderEnemy is not null)
                    _currentScene.Ignore(newUnderEnemy);

                enemy.Cube.MoveTo(ConvertIntoWorldCoordinates(enemyNewPosition) + new Vector3(0,Content.CubeSizeWithOffset,0), 0.05f, true);
                enemy.Position = enemyNewPosition;
            }

            private Cube? GetCube (Point position, IEnumerable<Cube> cubes) =>
                cubes.FirstOrDefault(c => c.Position == ConvertIntoWorldCoordinates(position));

            public Point ConvertIntoFieldCoordinates(Vector3 position) =>
                new (
                        (int)((position.X + _currentScene.CameraDelta.X) / View.Content.CubeSizeWithOffset + Size / 2),
                        (int)((position.Z + _currentScene.CameraDelta.Z) / View.Content.CubeSizeWithOffset + Size / 2)
                    );
                

            public Vector3 ConvertIntoWorldCoordinates(Point point) =>
                ConvertIntoWorldCoordinates(point.X, point.Y);

            public Vector3 ConvertIntoWorldCoordinates(int x, int y) =>
                new(View.Content.CubeSizeWithOffset * (x - Size / 2) - _currentScene.CameraDelta.X, 
                    0, View.Content.CubeSizeWithOffset * (y - Size / 2) - _currentScene.CameraDelta.Z);

            private void MakeField()
            {
                for (var i = 0; i < Size; i++)
                for (var j = 0; j < Size; j++)
                {
                    if (_fieldElements[i, j] == FieldElement.Void);
                        _fieldElements[i,j] = FieldElement.Empty;

                    if (i == 0 || j == 0 || i == Size - 1 || j == Size - 1)
                        _fieldElements[i, j] = FieldElement.Void;
                }
            }

            private void MakeFieldCircled()
            {
                _fieldElements[1, 1] = FieldElement.Void;
                _fieldElements[1, 2] = FieldElement.Void;
                _fieldElements[2, 1] = FieldElement.Void;

                _fieldElements[Size - 2, 1] = FieldElement.Void;
                _fieldElements[Size - 2, 2] = FieldElement.Void;
                _fieldElements[Size - 3, 1] = FieldElement.Void;

                _fieldElements[1, Size - 2] = FieldElement.Void;
                _fieldElements[2, Size - 2] = FieldElement.Void;
                _fieldElements[1, Size - 3] = FieldElement.Void;

                _fieldElements[Size - 2, Size - 2] = FieldElement.Void;
                _fieldElements[Size - 3, Size - 2] = FieldElement.Void;
                _fieldElements[Size - 2, Size - 3] = FieldElement.Void;
            }


            private void AddElements(IPositionsGenerator generator, FieldElement element)
            {
                foreach (var position in generator.GeneratePositions(Size))
                    _fieldElements[position.X, position.Y] = element;
                    
            }


            private void SpawnEnemies(IReadOnlyList<(IEnemyAlgorithm algorithm, Cube cube)> enemies)
            {
                var halfOfSize = Size / 2;

                switch (enemies.Count)
                {
                    case 1:
                        CreateEnemy(halfOfSize, halfOfSize, enemies[0]);
                        break;
                    case 2:
                        CreateEnemy(halfOfSize - 1, halfOfSize - 1, enemies[0]);
                        CreateEnemy(halfOfSize + 1, halfOfSize + 1, enemies[1]);
                        break;
                    case 3:
                        CreateEnemy(halfOfSize - 1, halfOfSize - 1, enemies[0]);
                        CreateEnemy(halfOfSize + 1, halfOfSize + 1, enemies[1]);
                        CreateEnemy(halfOfSize, halfOfSize, enemies[2]);
                        break;
                    case 4:
                        CreateEnemy(halfOfSize - 1, halfOfSize, enemies[0]);
                        CreateEnemy(halfOfSize + 1, halfOfSize, enemies[1]);
                        CreateEnemy(halfOfSize, halfOfSize + 1, enemies[2]);
                        CreateEnemy(halfOfSize, halfOfSize - 1, enemies[3]);
                        break;
                    case 5:
                        CreateEnemy(halfOfSize - 1, halfOfSize - 1, enemies[0]);
                        CreateEnemy(halfOfSize + 1, halfOfSize + 1, enemies[1]);
                        CreateEnemy(halfOfSize - 1, halfOfSize + 1, enemies[2]);
                        CreateEnemy(halfOfSize + 1, halfOfSize - 1, enemies[3]);
                        CreateEnemy(halfOfSize, halfOfSize, enemies[4]);
                        break;
                    default:
                        throw new ArgumentException("Unexpected enemies count: " + enemies.Count);
                }
            }

            private void CreateEnemy(int posX, int posY, (IEnemyAlgorithm algorithm, Cube cube) enemy)
            {
                _fieldElements[posX, posY] = FieldElement.Enemy;
                _enemies.Add(new Enemy(new Point(posX, posY), enemy.algorithm, enemy.cube));
            }
        }
    }
}
