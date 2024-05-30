using System;
using System.Collections.Generic;
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
            public const int PauseFormSize = 700;
            public const int ButtonSize = 100;
            public void Execute(Entity sender)
            {
                if (new StackTrace().GetFrame(1)!.GetMethod()!.DeclaringType != typeof(Controller))
                    throw new InvalidOperationException("Method can only be execute in controller");

                var entities = new List<Entity>();

                var resumeButton = new InterfaceButton(
                    new Position(50, 30, PositionType.Percents),
                    View.Content.ButtonRegular,
                    View.Content.ButtonHover,
                    View.Content.ButtonClick,
                    300, 100, 0, new ResumeGame(),
                    "Resume");

                var mainMenu = new InterfaceButton(
                    new Position(50, 50, PositionType.Percents),
                    View.Content.ButtonRegular,
                    View.Content.ButtonHover,
                    View.Content.ButtonClick,
                    300, 100, 0, new LoadMainMenu(),
                    "Main     menu");

                var exit = new InterfaceButton(
                    new Position(50, 70, PositionType.Percents),
                    View.Content.ButtonRegular,
                    View.Content.ButtonHover,
                    View.Content.ButtonClick,
                    300, 100, 0, new ExitGame(),
                    "Exit");

                entities.AddRange(new Entity[] {resumeButton, mainMenu, exit });
                entities.AddRange(GetPauseForm());

                _controller.PauseGame(entities);
            }

            public static List<Entity> GetPauseForm() =>
                GetPauseForm(PauseFormSize);

            public static List<Entity> GetPauseForm(int pauseFormSize)
            {
                var result = new List<Entity>();

                var pauseBackground = new Image(new Position(50, 50, PositionType.Percents),
                    new Sprite(Content.TranslucentBlack, Screen.Width, Screen.Height), 100);

                var pauseForeground = new Image(new Position(50, 50, PositionType.Percents),
                    new Sprite(Content.PauseTexture, pauseFormSize, pauseFormSize), 110);

                var closeButtonPosition = new Position(50, 50, PositionType.Percents)
                    .GetCoordinate(Screen, pauseFormSize, pauseFormSize) + new Vector2(pauseFormSize - ButtonSize / 2, ButtonSize);

                var closeButton = new InterfaceButton(
                    new Position(closeButtonPosition.X, closeButtonPosition.Y, PositionType.Pixels),
                    Content.CrossButtonRegular,
                    Content.CrossButtonHover,
                    Content.CrossButtonClick,
                    ButtonSize, ButtonSize, 120,
                    new ResumeGame());

                result.AddRange(new Entity[] { pauseBackground, pauseForeground, closeButton });
                return result;
            }
        }
    }
}
