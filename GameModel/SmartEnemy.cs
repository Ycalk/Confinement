using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using static Confinement.GameModel.GameModel.Field;
using static Confinement.GameModel.GameModel;

namespace Confinement.GameModel
{
    internal class SmartEnemy : IEnemyAlgorithm
    {
        #region Init
        private class DistanceField
        {
            public int this[Point point] => _distances[point.X, point.Y];

            private readonly int[,] _distances;
            private readonly Field _field;

            private int[,] GetDistances(Point start)
            {
                var distances = new int[_field.Size, _field.Size];
                for (var i = 0; i < _field.Size; i++)
                    for (var j = 0; j < _field.Size; j++)
                        distances[i, j] = int.MaxValue;
                distances[start.X, start.Y] = 0;
                var queue = new Queue<Point>();
                queue.Enqueue(start);
                while (queue.Count != 0)
                {
                    var current = queue.Dequeue();
                    foreach (var neighbour in GetNeighbours(current, _field))
                    {
                        if (distances[neighbour.X, neighbour.Y] != int.MaxValue) continue;
                        distances[neighbour.X, neighbour.Y] = distances[current.X, current.Y] + 1;
                        queue.Enqueue(neighbour);
                    }
                }
                return distances;
            }

            public DistanceField(Point start, Field field)
            {
                _field = field;
                _distances = GetDistances(start);
            }
        }

        private class NodeData
        {
            public Point? Previous { get; }
            public double Price { get; }

            public NodeData(Point? previous, double price)
            {
                Previous = previous;
                Price = price;
            }
        }

        private static List<Point> GetObstacles(Field field)
        {
            var result = new List<Point>();
            for (var i = 0; i < field.Size; i++)
                for (var j = 0; j < field.Size; j++)
                {
                    if (field[i, j] == FieldElement.Obstacle)
                        result.Add(new Point(i, j));
                }
            return result;
        }

        private class AlgorithmInfo
        {
            public AlgorithmInfo(Point start, Field field)
            {
                Field = field;
                Distances = new DistanceField(start, field);
                Track = new Dictionary<Point, NodeData> { [start] = new(null, 0) };
                Visited = new HashSet<Point>();
                NotVisited = GetNeighbours(start, field).ToHashSet();
                Obstacles = GetObstacles(field);
                Targets = new HashSet<Point>();
                for (var i = 0; i < field.Size; i++)
                    for (var j = 0; j < field.Size; j++)
                        if (field[i, j] == FieldElement.Void)
                            Targets.Add(new Point(i, j));
            }

            public Field Field { get; }
            public DistanceField Distances { get; }
            public Dictionary<Point, NodeData> Track { get; }
            public HashSet<Point> Visited { get; }
            public HashSet<Point> NotVisited { get; }
            public List<Point> Obstacles { get; }
            public HashSet<Point> Targets { get; }
        }

        #endregion
        #region NeighboursFinder
        private static IEnumerable<Point> GetDeltas()
        {
            for (var dy = -1; dy <= 1; dy++)
                for (var dx = -1; dx <= 1; dx++)
                {
                    if (dx != 0 && dy != 0) continue;
                    yield return new Point(dx, dy);
                }
        }

        private static bool InBounds(Point point, int fieldSize) =>
            point.X >= 0 && point.X < fieldSize && point.Y >= 0 && point.Y < fieldSize;

        private static void GetNeighbours(Point point, Field field, List<Point> neighbours, HashSet<Point> doubleMove)
        {
            foreach (var delta in GetDeltas())
            {
                var neighbour = delta + point;
                if (!InBounds(neighbour, field.Size) ||
                    field[neighbour.X, neighbour.Y] == FieldElement.Obstacle ||
                    field[neighbour.X, neighbour.Y] == FieldElement.Enemy)
                    continue;
                if (field[neighbour.X, neighbour.Y] == FieldElement.DoubleMove &&
                    doubleMove.Add(neighbour))
                    GetNeighbours(neighbour, field, neighbours, doubleMove);
                else
                    neighbours.Add(neighbour);
            }
        }

        private static IEnumerable<Point> GetNeighbours(Point point, Field field)
        {
            var result = new List<Point>();
            GetNeighbours(point, field, result, new HashSet<Point>());
            return result;
        }


        private static IEnumerable<Point> GetNeighbours(Point point, AlgorithmInfo info) =>
            GetNeighbours(point, info.Field);


        #endregion

        private static List<Point> GetPath(Point end,
            AlgorithmInfo info)
        {
            Point? current = end;
            var path = new List<Point>();
            while (current is not null)
            {
                path.Add((Point)current);
                current = info.Track[(Point)current].Previous;
            }
            path.Reverse();
            return path;
        }

        private static double GetDistance(Point first, Point second) =>
            Math.Sqrt((first.X - second.X) * (first.X - second.X) + (first.Y - second.Y) * (first.Y - second.Y));

        private static double GetPrice(Point point, AlgorithmInfo infos) =>
            infos.Obstacles
                .Select(obstacle => ((double)infos.Field.Size / 2 - GetDistance(point, obstacle)) * infos.Distances[point])
                .Where(counted => counted > 0).Sum();

        private static Point? GetNodeToOpen(AlgorithmInfo infos)
        {
            Point? result = null;
            var bestPrice = double.PositiveInfinity;
            foreach (var point in infos.NotVisited.Where(infos.Track.ContainsKey))
            {
                var currentPrice = infos.Track[point].Price;
                if (currentPrice >= bestPrice ||
                    infos.Visited.Contains(point)) continue;
                bestPrice = currentPrice;
                result = point;
            }
            return result;
        }

        private static void OpenNode(Point point, AlgorithmInfo info)
        {
            foreach (var neighbour in GetNeighbours(point, info))
            {
                info.NotVisited.Add(neighbour);
                var currentPrice = info.Track[point].Price + GetPrice(neighbour, info);
                if (!info.Track.TryGetValue(neighbour, out var neighbourData) || neighbourData.Price > currentPrice)
                    info.Track[neighbour] = new NodeData(point, currentPrice);
            }
        }


        private static IEnumerable<List<Point>> GetPath(Point start, Field field)
        {
            var info = new AlgorithmInfo(start, field);
            info.NotVisited.Add(start);
            while (info.NotVisited.Count != 0)
            {
                var toOpen = GetNodeToOpen(info);
                if (toOpen is null) yield break;
                var toOpenPoint = (Point)toOpen;
                if (info.Targets.Remove(toOpenPoint))
                    yield return GetPath(toOpenPoint, info);
                OpenNode(toOpenPoint, info);
                info.Visited.Add(toOpenPoint);
            }
        }

        public Point GetMove(Field elements, Point position)
        {
            var path = GetPath(position, elements).FirstOrDefault();
            return path is null ? position : path[1];
        }
    }
}
