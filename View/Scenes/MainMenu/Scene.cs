using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Architecture;
using Architecture.Entities;
using Architecture.Entities.System;
using Confinement.View.Scenes.MainMenu.Content;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

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

        public static Architecture.Scene GetScene()
        {
            var selectLevel = new MainMenuButton(
                new Position(50, 75, PositionType.Percents), 1,
                new Sprite(View.Content.ButtonRegular, ButtonsWidth, ButtonsHeight), new GameModel.GameModel.EnemyCountMenu(),
                "Select     level");


            var startButton = new MainMenuButton(
                new Position(50, 60, PositionType.Percents), 1,
                new Sprite(View.Content.ButtonRegular, ButtonsWidth, ButtonsHeight), new GameModel.GameModel.StartGame(3, 35),
                "Start");

            var exitButton = new MainMenuButton(
                new Position(50, 90, PositionType.Percents), 1,
                new Sprite(View.Content.ButtonRegular, ButtonsWidth, ButtonsHeight), new GameModel.GameModel.ExitGame(),
                "Exit");

            var gameImage = new Image(new Position(50, 20, PositionType.Percents),
                new Sprite(View.Content.MainMenuImage, 500, 500), 1);

            return new Scene(new[] { startButton, exitButton, selectLevel },
                new[] { gameImage },
                Array.Empty<Text>(),
                Array.Empty<Cube>(),
                Main.Graphics,
                Sprite.GeSolidColorTexture(Main.Graphics, Color.White,
                    GameModel.GameModel.Screen.Width, GameModel.GameModel.Screen.Height));
        }
    }
}
