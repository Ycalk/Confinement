using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Confinement.GameModel
{
    internal partial class GameModel
    {
        internal class ExitGame : IGamePlot
        {
            public void Execute()
            {
                if (new StackTrace().GetFrame(1)!.GetMethod()!.DeclaringType != typeof(Controller))
                    throw new InvalidOperationException("Method can only be execute in controller");
                _controller.Exit();
            }
        }
    }
}
