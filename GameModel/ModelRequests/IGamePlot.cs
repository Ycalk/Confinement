using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Architecture.Entities.System;

namespace Confinement.GameModel
{
    internal interface IGamePlot
    {
        public void Execute(Entity sender);
    }
}
