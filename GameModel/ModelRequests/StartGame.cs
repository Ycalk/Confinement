using Confinement.GameModel.PositionsGenerator;
using Confinement.View;
using Confinement.View.Scenes.Cubes.Content;
using Microsoft.Xna.Framework;

namespace Confinement.GameModel
{
    internal partial class GameModel
    {
        internal class StartGame : IGamePlot
        {
            public void Execute()
            {
                if (_state == GameState.MainMenu)
                {
                    var fieldSize = 65;
                    _state = GameState.Playing;
                    _field = new Field(fieldSize, 
                        new MazeScaling(2, .26),
                        new NormalDistribution(Field.FieldElement.DoubleMove, fieldSize),
                        (new SmartEnemy(), new EnemyCube()), 
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
