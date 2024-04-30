using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Architecture;
using Architecture.Entities;
using Architecture.Entities.System;
using Confinement.View.System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Confinement.View.Scenes.Cubes
{
    internal class Scene : Architecture.Scene, ISceneConstructor, ISceneWithCubes
    {
        public Scene(IEnumerable<Button> buttons, 
            IEnumerable<Image> images, 
            IEnumerable<Text> texts, 
            IEnumerable<Cube> cubes, 
            GraphicsDevice graphics, 
            Texture2D background) 
            : base(buttons, images, texts, cubes, graphics, background)
        {
        }

        public static Architecture.Scene GetScene()
        {
            var cubesPositions = new Vector3[]
            {
                new (0, 0, 0),
                new (View.Content.CubeSizeWithOffset,0,0),
                new (-View.Content.CubeSizeWithOffset,0,0),
                new (0,0,View.Content.CubeSizeWithOffset),
                new (0,0,-View.Content.CubeSizeWithOffset),
                new (View.Content.CubeSizeWithOffset,0,View.Content.CubeSizeWithOffset),
                new (-View.Content.CubeSizeWithOffset,0,View.Content.CubeSizeWithOffset),
                new (View.Content.CubeSizeWithOffset,0,-View.Content.CubeSizeWithOffset),
                new (-View.Content.CubeSizeWithOffset,0,-View.Content.CubeSizeWithOffset)
            };

            var cubes = cubesPositions.Select(p => new Content.Cube(p, View.Content.Cube)).ToList();
            return new Scene(
                Array.Empty<Button>(),
                Array.Empty<Image>(),
                Array.Empty<Text>(),
                cubes,
                Main.Graphics,
                Sprite.GeSolidColorTexture(Main.Graphics, Color.White, 
                    GameModel.GameModel.Screen.Width, GameModel.GameModel.Screen.Height)
            );
        }

        public void AddCube(Cube cube)
        {
            CubeManager.Add(cube);
        }

        public void RemoveCube(Cube cube)
        {
            CubeManager.Remove(cube);
        }

        public void IgnoreCube(Cube cube)
        {
            CubeManager.AddIgnoringCube(cube);
        }
    }
}
