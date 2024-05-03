

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

        public enum GameState
        {
            MainMenu,
            Playing,
        }

        public static GameState State { get; private set; }
        public static Screen Screen { get; private set; }

        private static PlayStay _playStay;
        private static Scene _currentScene;
        private static Controller _controller;

        static GameModel()
        {
            State = GameState.MainMenu;
        }
    }
}
