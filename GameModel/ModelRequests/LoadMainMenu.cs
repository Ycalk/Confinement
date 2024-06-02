using Confinement.View.Scenes.Cubes.Content;
using System.Diagnostics;
using System;
using Architecture.Entities.System;

namespace Confinement.GameModel
{
    internal partial class GameModel
    {
        internal class LoadMainMenu : IGamePlot
        {
            public void Execute(Entity sender)
            {
                if (new StackTrace().GetFrame(1)!.GetMethod()!.DeclaringType != typeof(Controller))
                    throw new InvalidOperationException("Method can only be execute in controller");
                _controller.StartModel();
                _state = GameState.MainMenu;
            }
        }
    }
}
