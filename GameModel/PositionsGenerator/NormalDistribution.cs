using System;
using System.Collections.Generic;
using MathNet.Numerics.Distributions;
using Microsoft.Xna.Framework;

namespace Confinement.GameModel.PositionsGenerator
{
    internal class NormalDistribution : IPositionsGenerator
    {
        private readonly GameModel.Field.FieldElement _element;
        private readonly int _count;

        public NormalDistribution(GameModel.Field.FieldElement element, int count)
        {
            if (element is not (GameModel.Field.FieldElement.DoubleMove or GameModel.Field.FieldElement.Obstacle))
                throw new ArgumentOutOfRangeException(nameof(element), element,
                    "Only DoubleMove and Obstacle elements are supported");
            _element = element;
            _count = count;
        }

        private static int GetDoubleMoveDispersion(int fieldSize) =>
            fieldSize / 6;

        private static int GetObstacleDispersion (int fieldSize) =>
            fieldSize * 3 / 2;

        public IEnumerable<Point> GeneratePositions(int fieldSize)
        {
            var center = new Point(fieldSize / 2, fieldSize / 2);
            var count = 0;
            var stdDev = _element == GameModel.Field.FieldElement.DoubleMove ? GetDoubleMoveDispersion(fieldSize) : GetObstacleDispersion(fieldSize);

            var normalDistX = new Normal(center.X, stdDev);
            var normalDistY = new Normal(center.Y, stdDev);

            while (count < _count)
            {
                var x = normalDistX.Sample();
                var y = normalDistY.Sample();

                if (x >= 1 && x < fieldSize - 2 && y >= 1 && y < fieldSize - 2)
                {
                    yield return new Point((int)Math.Round(x), (int)Math.Round(y));
                    count++;
                }
                    
            }
        }
    }
}
