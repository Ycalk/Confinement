using Confinement.GameModel.PositionsGenerator;
using Confinement.View;
using Confinement.View.Scenes.Cubes.Content;
using Microsoft.Xna.Framework;
using System.Diagnostics;
using System;

namespace Confinement.GameModel
{
    internal partial class GameModel
    {
        internal class StartGame : IGamePlot
        {
            public void Execute()
            {
                if (new StackTrace().GetFrame(1)!.GetMethod()!.DeclaringType != typeof(Controller))
                    throw new InvalidOperationException("Method can only be execute in controller");

                if (_state == GameState.MainMenu)
                {
                    var fieldSize = 45;
                    _state = GameState.Playing;
                    _field = new Field(fieldSize, 
                        new MazeScaling(2, .25),
                        new NormalDistribution(Field.FieldElement.DoubleMove, fieldSize * 3),
                        (new SmartEnemy(), new EnemyCube()), 
                        (new SmartEnemy(), new EnemyCube()),
                        (new SmartEnemy(), new EnemyCube()),
                        (new SmartEnemy(), new EnemyCube()));
                    _playState = PlayState.PlayerMove;
                    _controller.LoadScene(View.Scenes.Cubes.Scene.GetScene(_field, 70));
                }
            }
        }
    }
}
