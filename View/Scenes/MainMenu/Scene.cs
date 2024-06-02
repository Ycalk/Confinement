using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Architecture;
using Architecture.Entities;
using Architecture.Entities.System;
using Confinement.GameModel;
using Confinement.GameModel.GameModes;
using Confinement.GameModel.PositionsGenerator;
using Confinement.View.Scenes.Cubes.Content;
using Confinement.View.Scenes.MainMenu.Content;
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

        private static List<Entity> GetMainMenu()
        {
            var gameImage = new Image(new Position(5, 10, PositionType.Percents),
                new Sprite(View.Content.MainMenuImage, 500, 500), 1);

            var startButton = new MainMenuButton(
                new Position(10, 50, PositionType.Percents), 1,
                new Sprite(View.Content.ButtonRegular, ButtonsWidth, ButtonsHeight), new StartGame(1, new Classic()),
                "Start");

            var selectLevel = new MainMenuButton(
                new Position(10, 65, PositionType.Percents), 1,
                new Sprite(View.Content.ButtonRegular, ButtonsWidth, ButtonsHeight), new GameModel.GameModel.EnemyCountMenu(),
                "Select     level");


            var exitButton = new MainMenuButton(
                new Position(10, 80, PositionType.Percents), 1,
                new Sprite(View.Content.ButtonRegular, ButtonsWidth, ButtonsHeight), new GameModel.GameModel.ExitGame(),
                "Exit");

            

            return new List<Entity>() { selectLevel, startButton, exitButton, gameImage };
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
