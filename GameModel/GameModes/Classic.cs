using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Confinement.GameModel.GameModes
{
    internal class Classic : IGameMode
    {
        public int CurrentLevel { get; set; } = 1;

        public void OnPlayerWin()
        {
            var menuButtons = LevelSelect.CreateButtons(
                CurrentLevel < GameModel.Field.MaximalEnemyCount, 
                new GameModel.StartGame(1, new Classic()), this, false);
            var request = new ModelRequest(
                new GameModel.LoadGameOverMenu(LevelSelect.GameOverMenuWidth, LevelSelect.GameOverMenuHeight, menuButtons,
                    LevelSelect.GetText(CurrentLevel + "/" + GameModel.Field.MaximalEnemyCount 
                                                 + " completed")), null);
            GameModel.Controller.CreateRequest(request);
        }

        public void OnPlayerLose()
        {
            var menuButtons = LevelSelect.CreateButtons(false, 
                new GameModel.StartGame(1, new Classic()), this);
            var request = new ModelRequest(
                new GameModel.LoadGameOverMenu(LevelSelect.GameOverMenuWidth, LevelSelect.GameOverMenuHeight, menuButtons,
                    LevelSelect.GetText("Your score: "+CurrentLevel)), null);
            GameModel.Controller.CreateRequest(request);
        }
    }
}
