using System;
using System.Collections.Generic;
using System.Diagnostics;
using Architecture.Entities;
using Architecture.Entities.System;
using Confinement.View;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Confinement.GameModel
{
    internal partial class GameModel
    {
        internal class PauseGame : IGamePlot
        {
            public void Execute()
            {
                if (new StackTrace().GetFrame(1)!.GetMethod()!.DeclaringType != typeof(Controller))
                    throw new InvalidOperationException("Method can only be execute in controller");

                var entities = new List<Entity>();

                var pauseBackground = new Image(new Position(50, 50, PositionType.Percents),
                    new Sprite(Content.TranslucentBlack, Screen.Width, Screen.Height), 100);

                var pauseForeground = new Image(new Position(50, 50, PositionType.Percents),
                    new Sprite(Content.PauseTexture, 700, 700), 110);

                var resumeButton = new View.Scenes.Cubes.Content.PauseButton(
                    new Position(50, 30, PositionType.Percents),
                    View.Content.ButtonRegular,
                    View.Content.ButtonHover,
                    View.Content.ButtonClick,
                    300, 100, 0, new ResumeGame(),
                    "Resume");

                var mainMenu = new View.Scenes.Cubes.Content.PauseButton(
                    new Position(50, 50, PositionType.Percents),
                    View.Content.ButtonRegular,
                    View.Content.ButtonHover,
                    View.Content.ButtonClick,
                    300, 100, 0, new LoadMainMenu(),
                    "Main Menu");

                var exit = new View.Scenes.Cubes.Content.PauseButton(
                    new Position(50, 70, PositionType.Percents),
                    View.Content.ButtonRegular,
                    View.Content.ButtonHover,
                    View.Content.ButtonClick,
                    300, 100, 0, new ExitGame(),
                    "Exit");

                entities.AddRange(new Entity[] {pauseBackground, pauseForeground, resumeButton, mainMenu, exit });

                _controller.PauseGame(entities);
            }
        }
    }
}
