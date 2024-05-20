using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Confinement.GameModel
{
    internal partial class GameModel
    {
        internal class ExitButtonPress : IGamePlot
        {
            public void Execute()
            {
                _controller.Exit();
            }
        }
    }
}
