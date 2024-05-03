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
                    State = GameState.Playing;
                    _controller.LoadScene(View.Scenes.Cubes.Scene.GetScene());
                }
            }
        }
    }

    
}
