using Confinement.View.Scenes.Cubes.Content;

namespace Confinement.GameModel
{
    internal partial class GameModel
    {
        internal class LoadMainMenu : IGamePlot
        {
            public void Execute()
            {
                _controller.LoadScene(View.Scenes.MainMenu.Scene.GetScene());
                _state = GameState.MainMenu;
            }
        }
    }
}
