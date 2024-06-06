using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using Architecture.Entities;
using Architecture.Entities.System;
using Confinement.GameModel.GameModes;
using Confinement.View;
using Confinement.View.Scenes.Cubes.Content;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Confinement.GameModel
{
    internal partial class GameModel
    {
        internal class PauseGame : IGamePlot
        {
            public const int ButtonSize = 100;
            public const float MovingTime = 0.0003f;
            public const int PauseFormSize = 630;
            public static Vector2 Delta => new(0, (Screen.Height + PauseFormSize) * 0.5f);

            public void Execute(Entity sender)
            {
                if (new StackTrace().GetFrame(1)!.GetMethod()!.DeclaringType != typeof(Controller))
                    throw new InvalidOperationException("Method can only be execute in controller");

                var padding = 150;
                var buttonsSize = 120;

                var entities = new List<Entity>();

                
                var mainMenuPosition = new Position(50, 50, PositionType.Percents)
                    .GetCoordinate(Screen, 300, 100);

                var resumeButtonPosition = new Position(50, 50, PositionType.Percents)
                    .GetCoordinate(Screen, 300, 100) + new Vector2(0, -padding);

                var buttons = new Position(50, 50, PositionType.Percents)
                    .GetCoordinate(Screen, 300, 100) + new Vector2(0 , padding);


                var resumeButton = new InterfaceButton(
                    new Position(resumeButtonPosition - Delta, PositionType.Pixels),
                    View.Content.ButtonRegular,
                    View.Content.ButtonHover,
                    View.Content.ButtonClick,
                    300, 100, 0, new ResumeGame(),
                    "Resume");

                var mainMenu = new InterfaceButton(
                    new Position(mainMenuPosition - Delta, PositionType.Pixels),
                    View.Content.ButtonRegular,
                    View.Content.ButtonHover,
                    View.Content.ButtonClick,
                    300, 100, 0, new LoadMainMenu(),
                    "Main     menu");

                var exitButton = new InterfaceButton(
                    new Position(buttons - Delta, PositionType.Pixels),
                    View.Content.ExitButtonRegular,
                    View.Content.ExitButtonHover,
                    View.Content.ExitButtonClick,
                    buttonsSize, buttonsSize, 0, new ExitGame());

                var restartPlot = _gameMode switch
                {
                    Classic => new StartGame(1, new Classic()),
                    LevelSelect => new StartGame(_gameMode.CurrentLevel),
                    _ => throw new NotImplementedException("Unknown game mode")
                };

                var restartButton = new InterfaceButton(
                    new Position(buttons - Delta + new Vector2(300 - buttonsSize, 0), PositionType.Pixels),
                    View.Content.RefreshButtonRegular,
                    View.Content.RefreshButtonHover,
                    View.Content.RefreshButtonClick,
                    120, 120, 0, restartPlot);

                resumeButton.MoveTo(
                    Screen, resumeButtonPosition, MovingTime, true);

                mainMenu.MoveTo(
                    Screen, mainMenuPosition, MovingTime, true);

                exitButton.MoveTo(
                    Screen, buttons, MovingTime, true);

                restartButton.MoveTo(
                    Screen, buttons + new Vector2(300 - buttonsSize, 0), MovingTime, true);

                entities.AddRange(new Entity[] {resumeButton, mainMenu, exitButton, restartButton });
                entities.AddRange(GetPauseForm());

                _controller.PauseGame(entities);
            }

            public static List<Entity> GetPauseForm() =>
                GetPauseForm(PauseFormSize);

            public static List<Entity> GetPauseForm(int pauseFormSize)
            {
                var result = new List<Entity>();

                var foregroundPosition = new Position(50, 50, PositionType.Percents)
                    .GetCoordinate(Screen, pauseFormSize, pauseFormSize);

                var closeButtonPosition = new Position(50, 50, PositionType.Percents)
                    .GetCoordinate(Screen, pauseFormSize, pauseFormSize) + new Vector2(pauseFormSize - ButtonSize / 2, ButtonSize);

                var pauseBackground = new Image(new Position(50, 50, PositionType.Percents),
                    new Sprite(Content.TranslucentBlack, Screen.Width, Screen.Height), 100);

                var pauseForeground = new Image(new Position(foregroundPosition - Delta, PositionType.Pixels),
                    new Sprite(Content.PauseTexture, pauseFormSize, pauseFormSize), 110);

                var closeButton = new InterfaceButton(
                    new Position(closeButtonPosition - Delta, PositionType.Pixels),
                    Content.CrossButtonRegular,
                    Content.CrossButtonHover,
                    Content.CrossButtonClick,
                    ButtonSize, ButtonSize, 120,
                    new ResumeGame());

                pauseForeground.MoveTo(
                    Screen, foregroundPosition, MovingTime, true);

                closeButton.MoveTo(
                    Screen, closeButtonPosition, MovingTime, true);

                result.AddRange(new Entity[] { pauseBackground, pauseForeground, closeButton });
                return result;
            }
        }
    }
}
