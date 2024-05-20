

using Architecture;

namespace Confinement.GameModel
{
    internal static partial class GameModel
    {
        private enum PlayState
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

        private static GameState _state;
        public static Screen Screen { get; private set; }

        private static PlayState _playState;
        private static Scene _currentScene;
        private static Controller _controller;
        private static Field _field;

        static GameModel()
        {
            _state = GameState.MainMenu;
        }
    }
}
