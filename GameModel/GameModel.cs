

using Architecture;

namespace Confinement.GameModel
{
    internal static partial class GameModel
    {
        private enum PlayStay
        {
            PlayerMove,
            ComputerMove,
            PlayerWin,
            ComputerWin,
        }

        public static State GameState { get; private set; }
        public static Screen Screen { get; private set; }

        private static PlayStay _playStay;
        private static Scene _currentScene;

        static GameModel()
        {
            GameState = State.MainMenu;
        }
    }
}
