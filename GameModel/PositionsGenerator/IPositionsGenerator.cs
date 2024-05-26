using System.Collections.Generic;
using Microsoft.Xna.Framework;
using static Confinement.GameModel.GameModel.Field;

namespace Confinement.GameModel.PositionsGenerator
{
    internal interface IPositionsGenerator
    {
        public IEnumerable<Point> GeneratePositions(int fieldSize);
    }
}
