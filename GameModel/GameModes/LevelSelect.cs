﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Architecture;
using Architecture.Entities;
using Architecture.Entities.System;
using Confinement.View;
using Confinement.View.Scenes.Cubes.Content;
using Microsoft.Xna.Framework;
using static System.Net.Mime.MediaTypeNames;
using Text = Architecture.Entities.Text;

namespace Confinement.GameModel.GameModes
{
    internal class LevelSelect : IGameMode
    {
        public int CurrentLevel { get; set; }
        public const int GameOverMenuWidth = 400;
        public const int GameOverMenuHeight = 300;


        public LevelSelect(int selectedLevel)
        {
            CurrentLevel = selectedLevel;
        }

        public void OnPlayerWin()
        {
            var buttons = CreateButtons(CurrentLevel < GameModel.Field.MaximalEnemyCount, 
                new GameModel.StartGame(CurrentLevel), this);
            var request = new ModelRequest(
                new GameModel.LoadGameOverMenu(GameOverMenuWidth, GameOverMenuHeight, buttons,
                    GetText("Victory")), null);
            GameModel.Controller.CreateRequest(request);
        }

        public void OnPlayerLose()
        {
            var buttons = CreateButtons(false, 
                new GameModel.StartGame(CurrentLevel), this);
            var request = new ModelRequest(
                new GameModel.LoadGameOverMenu(GameOverMenuWidth, GameOverMenuHeight, buttons,
                    GetText("Defeat")), null);
            GameModel.Controller.CreateRequest(request);
        }

        public static Text GetText(string value)
        {
            value = value.Replace(" ", "     ");
            var (width, height) = Content.GameOver.MeasureString(value);
            var menuPosition = new Position(50, 50, PositionType.Percents)
                .GetCoordinate(GameModel.Screen, GameOverMenuWidth, GameOverMenuHeight);

            var textX = new Position(50, 50, PositionType.Percents)
                .GetCoordinate(GameModel.Screen, (int)width, (int)height).X;

            return new Text(
                new Position(textX, menuPosition.Y + 50, PositionType.Pixels), 10, Content.GameOver, Color.Gray, value);
        }

        public static IEnumerable<Button> CreateButtons(bool withNextLevel, IGamePlot onRestartPress, IGameMode gameMode, bool restartButton = true )
        {
            var result = new List<Button>();
            var buttonsGap = 20;
            var buttonsSize = 100;
            var buttonsBottomIndent = 40;

            var buttonsCount = 1 + Convert.ToInt32(withNextLevel) + Convert.ToInt32(restartButton);
            var blockWidth = buttonsCount * buttonsSize + (buttonsCount - 1) * buttonsGap;

            var menuPosition = new Position(50, 50, PositionType.Percents)
                .GetCoordinate(GameModel.Screen, GameOverMenuWidth, GameOverMenuHeight);

            var startPosition =
                new Position(50, 50, PositionType.Percents).GetCoordinate(GameModel.Screen, blockWidth,
                    buttonsSize);

            var positionY = menuPosition.Y + GameOverMenuHeight - buttonsBottomIndent - buttonsSize;
            var startX = startPosition.X;

            var mainMenu = new InterfaceButton(new Position(startX, positionY, PositionType.Pixels),
                Content.MainMenuButtonRegular, 
                Content.MainMenuButtonHover, 
                Content.MainMenuButtonClick,
                buttonsSize, buttonsSize, 0,
                new GameModel.LoadMainMenu());
            result.Add(mainMenu);
            startX += buttonsSize + buttonsGap;

            if (restartButton)
            {
                var restart = new InterfaceButton(new Position(startX, positionY, PositionType.Pixels),
                    Content.RefreshButtonRegular,
                    Content.RefreshButtonHover,
                    Content.RefreshButtonClick, buttonsSize, buttonsSize, 0,
                    onRestartPress);
                result.Add(restart);
                startX += buttonsSize + buttonsGap;
            }
            
            
            if (withNextLevel)
            {
                var nextLevel = new InterfaceButton(new Position(startX, positionY, PositionType.Pixels),
                    Content.NextButtonRegular,
                    Content.NextButtonHover,
                    Content.NextButtonClick, buttonsSize, buttonsSize, 0,
                    new GameModel.StartGame(gameMode.CurrentLevel + 1, null, 
                        _ => gameMode.CurrentLevel += 1));
                result.Add(nextLevel);
            }

            return result;
        }
    }
}
