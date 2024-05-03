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
        public Scene(IEnumerable<Button> buttons,
            IEnumerable<Image> images, IEnumerable<Text> texts,
            IEnumerable<Cube> cubes,
            GraphicsDevice graphics, Texture2D background)
            : base(buttons, images, texts, cubes, graphics, background)
        {
        }

        public static Architecture.Scene GetScene()
        {
            var startButton = new StartButton(Main.Graphics,
                new Position(50, 50, PositionType.Percents), 1,
                new Sprite(Main.Graphics, Color.Black, 300, 50));

            var gameName = new Text(new Position(50, 10, PositionType.Percents), 0,
                View.Content.RegularFont, Color.Black, Main.GameName);

            return new Scene(new[] { startButton },
                Array.Empty<Image>(),
                new[] { gameName },
                Array.Empty<Cube>(),
                Main.Graphics,
                Sprite.GeSolidColorTexture(Main.Graphics, Color.White,
                    GameModel.GameModel.Screen.Width, GameModel.GameModel.Screen.Height));
        }
    }
}
