using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Architecture.Entities;
using Microsoft.Xna.Framework;
using static Confinement.GameModel.GameModel.Field;

namespace Confinement.GameModel
{
    internal interface IEnemyAlgorithm
    {
        public Point GetMove(GameModel.Field elements, Point position);

    }
}
