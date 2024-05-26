using System.Diagnostics;
using System;

namespace Confinement.GameModel
{
    internal partial class GameModel
    {
        internal class ResumeGame : IGamePlot
        {
            public void Execute()
            {
                if (new StackTrace().GetFrame(1)!.GetMethod()!.DeclaringType != typeof(Controller))
                    throw new InvalidOperationException("Method can only be execute in controller");
                _controller.UnPauseGame();
            }
        }
    }
}
