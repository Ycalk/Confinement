namespace Confinement.GameModel
{
    internal partial class GameModel
    {
        internal class ResumeGame : IGamePlot
        {
            public void Execute()
            {
                _controller.UnPauseGame();
            }
        }
    }
}
