using System.Diagnostics;
using System;
using Microsoft.Xna.Framework;

namespace Confinement.GameModel
{
    internal partial class GameModel
    {
        internal class ChangeCameraTarget : IGamePlot
        {
            public readonly Field.Enemy Target;

            public void Execute()
            {
                if (new StackTrace().GetFrame(1)!.GetMethod()!.DeclaringType != typeof(Controller))
                    throw new InvalidOperationException("Method can only be execute in controller");
                var target = new Vector3(Target.WorldPosition.X, 0, Target.WorldPosition.Z);
                _currentScene.ChangeCameraTarget(target);
            }

            public ChangeCameraTarget(Field.Enemy enemy)
            {
                Target = enemy;
            }
        }
    }
}
