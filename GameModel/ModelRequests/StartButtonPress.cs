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
                    var fieldSize = 11;
                    State = GameState.Playing;
                    var enemy = new EnemyCube();
                    _field = new Field(fieldSize, (new SmartEnemy(), enemy));
                    _playStay = PlayStay.PlayerMove;
                    _controller.LoadScene(View.Scenes.Cubes.Scene.GetScene(_field));
                    _currentScene.Ignore(enemy);
                }
            }

            
        }
    }

    
}
