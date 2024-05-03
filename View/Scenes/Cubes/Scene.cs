using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Architecture;
using Architecture.Entities;
using Architecture.Entities.System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Confinement.View.Scenes.Cubes
{
    internal class Scene : Architecture.Scene, ISceneConstructor
    {
        public Scene(IEnumerable<Button> buttons, 
            IEnumerable<Image> images, 
            IEnumerable<Text> texts, 
            IEnumerable<Cube> cubes, 
            GraphicsDevice graphics,
            float distancing,
            Texture2D background) 
            : base(buttons, images, texts, cubes, distancing, graphics, background)
        {
        }

        public static Architecture.Scene GetScene()
        {
            var fieldSize = 9;
            var cubesPositions = new List<Vector3>();
            for (var x = -(fieldSize / 2); x <= fieldSize / 2; x++)
            {
                for (var y = -(fieldSize / 2); y <= fieldSize / 2; y++)
                {
                    cubesPositions.Add(
                        new Vector3(View.Content.CubeSizeWithOffset * x, 0, View.Content.CubeSizeWithOffset * y));
                }
            }
            var cubes = cubesPositions.Select(p => new Content.Cube(p, View.Content.Cube));
            return new Scene(
                Array.Empty<Button>(),
                Array.Empty<Image>(),
                Array.Empty<Text>(),
                cubes,
                Main.Graphics,
                40,
                Sprite.GeSolidColorTexture(Main.Graphics, Color.White, 
                    GameModel.GameModel.Screen.Width, GameModel.GameModel.Screen.Height)
            );
        }
    }
}
