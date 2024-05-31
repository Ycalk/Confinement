

using Architecture;
using Confinement.GameModel.GameModes;

namespace Confinement.GameModel
{
    internal static partial class GameModel
    {
        private enum PlayState
        {
            PlayerMove,
            ComputerMove,
            Pause,
            PlayerWin,
            ComputerWin,
            GameOver
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
        private static IGameMode _gameMode;

        static GameModel()
        {
            _state = GameState.MainMenu;
        }
    }
}
