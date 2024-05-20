using Confinement.View;
using Confinement.View.Scenes.Cubes.Content;
using Microsoft.Xna.Framework;

namespace Confinement.GameModel
{
    internal partial class GameModel
    {
        internal class StartButtonPress : IGamePlot
        {
            public void Execute()
            {
                if (State == GameState.MainMenu)
                {
                    var fieldSize = 31;
                    State = GameState.Playing;
                    _field = new Field(fieldSize, 
                        (new SmartEnemy(), new EnemyCube()), 
                        (new SmartEnemy(), new EnemyCube()),
                        (new SmartEnemy(), new EnemyCube()));
                    _playStay = PlayStay.PlayerMove;
                    _controller.LoadScene(View.Scenes.Cubes.Scene.GetScene(_field, 70));
                }
            }

            
        }
    }

    
}
