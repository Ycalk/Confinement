using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using Architecture.Entities;
using Architecture.Entities.System;
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

                var entities = new List<Entity>();

                
                var mainMenuPosition = new Position(50, 50, PositionType.Percents)
                    .GetCoordinate(Screen, 300, 100);

                var resumeButtonPosition = new Position(50, 50, PositionType.Percents)
                    .GetCoordinate(Screen, 300, 100) + new Vector2(0, -padding);

                var exitPosition = new Position(50, 50, PositionType.Percents)
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

                var exit = new InterfaceButton(
                    new Position(exitPosition - Delta, PositionType.Pixels),
                    View.Content.ButtonRegular,
                    View.Content.ButtonHover,
                    View.Content.ButtonClick,
                    300, 100, 0, new ExitGame(),
                    "Exit");

                resumeButton.MoveTo(
                    Screen, resumeButtonPosition, MovingTime, true);

                mainMenu.MoveTo(
                    Screen, mainMenuPosition, MovingTime, true);

                exit.MoveTo(
                    Screen, exitPosition, MovingTime, true);

                entities.AddRange(new Entity[] {resumeButton, mainMenu, exit });
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
