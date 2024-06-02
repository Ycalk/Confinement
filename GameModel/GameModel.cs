

using System.Collections.Generic;
using System.Collections.ObjectModel;
using Architecture;
using Confinement.GameModel.GameModes;
using Confinement.GameModel.PlayerProgress;

namespace Confinement.GameModel
{
    internal static partial class GameModel
    {
        private static readonly PlayerProgress.PlayerProgress PlayerProgress;
        public static ReadOnlyCollection<int> CompletedLevels => PlayerProgress.CompletedLevels.AsReadOnly();

        public static void AddCompletedLevel(int level)
        {
            PlayerProgress.CompletedLevels.Add(level);
            SaveLoadManager.SaveProgress(PlayerProgress);
        }

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
            PlayerProgress = SaveLoadManager.LoadProgress();
        }
    }
}
