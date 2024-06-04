using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Architecture;
using Architecture.Entities;
using Architecture.Entities.System;
using Confinement.GameModel;
using Confinement.GameModel.GameModes;
using Confinement.GameModel.PositionsGenerator;
using Confinement.View.Scenes.Cubes.Content;
using Confinement.View.Scenes.MainMenu.Content;
using MathNet.Numerics.Statistics.Mcmc;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using static Confinement.GameModel.GameModel;
using Cube = Architecture.Entities.Cube;

namespace Confinement.View.Scenes.MainMenu
{
    internal class Scene : Architecture.Scene, ISceneConstructor
    {
        public const int ButtonsWidth = 300;
        public const int ButtonsHeight = 100;

        public Scene(IEnumerable<Button> buttons,
            IEnumerable<Image> images, IEnumerable<Text> texts,
            IEnumerable<Cube> cubes,
            GraphicsDevice graphics, Texture2D background)
            : base(buttons, images, texts, cubes, graphics, background)
        {
        }

        public static Architecture.Scene GetScene(GameModel.GameModel.Field field)
        {
            var scene = Cubes.Scene.GetScene(field, 50, 0,false);
            scene.Update(new GameTime(), GameModel.GameModel.Screen);
            var cubes = scene.GetEntities<Cube>().ToArray();
            foreach (var cube in cubes)
                scene.Ignore(cube);

            scene.ChangeCameraTarget(new Vector3(5, 0, 8), 0f);

            foreach (var entity in GetMainMenu())
                scene.Add(entity);
            
            return scene;
        }

        private static List<Entity2D> GetMainMenu()
        {
            var deltaX = GameModel.GameModel.Screen.Width / 4;
            var movingTime = 0.001f;
            var gameImageGoalPosition = new Position(5, 10, PositionType.Percents).GetCoordinate(GameModel.GameModel.Screen, 500, 500);
            var startButtonGoalPosition = new Position(10, 50, PositionType.Percents)
                .GetCoordinate(GameModel.GameModel.Screen, ButtonsWidth, ButtonsHeight);
            var selectLevelGoalPosition = new Position(10, 65, PositionType.Percents)
                .GetCoordinate(GameModel.GameModel.Screen, ButtonsWidth, ButtonsHeight);
            var exitButtonGoalPosition = new Position(10, 80, PositionType.Percents)
                .GetCoordinate(GameModel.GameModel.Screen, ButtonsWidth, ButtonsHeight);

            var gameImage = new Image(new Position(gameImageGoalPosition.X - deltaX, gameImageGoalPosition.Y, PositionType.Pixels),
                new Sprite(View.Content.MainMenuImage, 500, 500), 1);

            var startButton = new MainMenuButton(
                new Position(startButtonGoalPosition.X - deltaX, startButtonGoalPosition.Y, PositionType.Pixels), 1,
                new Sprite(View.Content.ButtonRegular, ButtonsWidth, ButtonsHeight), new StartGame(1, new Classic()),
                "Start");

            var selectLevel = new MainMenuButton(
                new Position(selectLevelGoalPosition.X - deltaX, selectLevelGoalPosition.Y, PositionType.Pixels), 1,
                new Sprite(View.Content.ButtonRegular, ButtonsWidth, ButtonsHeight), new GameModel.GameModel.EnemyCountMenu(),
                "Select     level");

            var exitButton = new MainMenuButton(
                new Position(exitButtonGoalPosition.X - deltaX, exitButtonGoalPosition.Y, PositionType.Pixels), 1,
                new Sprite(View.Content.ButtonRegular, ButtonsWidth, ButtonsHeight), new GameModel.GameModel.ExitGame(),
                "Exit");

            gameImage.MoveTo(GameModel.GameModel.Screen,
                gameImageGoalPosition,
                movingTime, true);

            startButton.MoveTo(GameModel.GameModel.Screen,
                startButtonGoalPosition,
                movingTime * 1.2f, true);

            selectLevel.MoveTo(GameModel.GameModel.Screen,
                selectLevelGoalPosition,
                movingTime * 1.4f, true);

            exitButton.MoveTo(GameModel.GameModel.Screen,
                exitButtonGoalPosition,
                movingTime * 1.6f, true);

            return new List<Entity2D>() { selectLevel, startButton, exitButton, gameImage };
        }

        public static Architecture.Scene GetScene()
        {
            var result = Empty(Main.Graphics);

            foreach (var entity in GetMainMenu())
                result.Add(entity);
            return result;
        }
    }
}
