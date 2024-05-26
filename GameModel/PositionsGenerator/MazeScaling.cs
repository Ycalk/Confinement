using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace Confinement.GameModel.PositionsGenerator
{
    internal class MazeScaling : IPositionsGenerator
    {
        public const int MazeGenerationIterations = 30;
        private readonly double _scalingChance;

        private class CellularAutomaton
        {
            public bool[,] Maze;
            private readonly int _size;
            public CellularAutomaton(int size, IEnumerable<Point> startPoints)
            {
                Maze = new bool[size, size];
                _size = size;
                foreach (var point in startPoints)
                    Maze[point.X, point.Y] = true;
            }

            private IEnumerable<Point> GetNeighbours(int x, int y)
            {
                for (var dy = -1; dy <= 1; dy++)
                for (var dx = -1; dx <= 1; dx++)
                    yield return new Point(dx + x, dy + y);
            }

            public void MakeIteration()
            {
                var newMaze = new bool[_size, _size];
                for (var i = 1; i < _size - 1; i++)
                for (var j = 1; j < _size - 1; j++)
                {
                    var count = GetNeighbours(i, j).Count(neighbour => Maze[neighbour.X, neighbour.Y]);
                    if (Maze[i, j])
                    {
                        count -= 1;
                        newMaze[i, j] = count is >= 1 and <= 5;
                    }
                    else
                        newMaze[i, j] = count == 3;
                }
                Maze = newMaze;
            }
        }

        private readonly int _scalingCoefficient;

        public MazeScaling(int scalingCoefficient, double scalingChance)
        {
            if (scalingChance is < 0 or > 1)
                throw new ArgumentOutOfRangeException(nameof(scalingChance), scalingChance,
                    "Scaling chance should be in [0, 1] range");
            _scalingCoefficient = scalingCoefficient;
            _scalingChance = scalingChance;
        }

        public IEnumerable<Point> GeneratePositions(int fieldSize)
        {
            var maze = GetMaze(fieldSize / _scalingCoefficient + 1);
            
            var pointsToScale = new NormalDistribution(GameModel.Field.FieldElement.Obstacle, 
                    (int)(fieldSize * fieldSize * _scalingChance))
                .GeneratePositions(fieldSize).ToHashSet();

            var ignoringPoints = new NormalDistribution(GameModel.Field.FieldElement.DoubleMove, 
                    (int)(fieldSize * 1 - _scalingChance)).GeneratePositions(fieldSize).ToHashSet();

            for (var i = 0; i < fieldSize; i++)
            for (var j = 0; j < fieldSize; j++)
            {
                var point = new Point(i, j);
                if (maze[i / _scalingCoefficient, j / _scalingCoefficient]
                    && pointsToScale.Contains(point)
                    && !ignoringPoints.Contains(point))
                    yield return point;
            }
                
        }

        private bool[,] GetMaze(int mazeSize)
        {
            var maze = new CellularAutomaton(mazeSize, 
                new NormalDistribution(GameModel.Field.FieldElement.Obstacle, mazeSize * mazeSize / 3)
                .GeneratePositions(mazeSize + 1));
            for (var i = 0; i < MazeGenerationIterations; i++)
                maze.MakeIteration();
            return maze.Maze;
        }
    }
}
