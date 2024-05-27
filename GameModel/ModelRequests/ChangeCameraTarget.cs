using System.Diagnostics;
using System;
using Microsoft.Xna.Framework;

namespace Confinement.GameModel
{
    internal partial class GameModel
    {
        internal class ChangeCameraTarget : IGamePlot
        {
            private readonly Vector3 _newTarget;

            public void Execute()
            {
                if (new StackTrace().GetFrame(1)!.GetMethod()!.DeclaringType != typeof(Controller))
                    throw new InvalidOperationException("Method can only be execute in controller");
                
                _currentScene.ChangeCameraTarget(_newTarget);
            }

            public ChangeCameraTarget(Vector3 newTarget)
            {
                _newTarget = newTarget;
            }
        }
    }
}
