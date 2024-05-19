using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Security.Cryptography.X509Certificates;
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

        public static Architecture.Scene GetScene(GameModel.GameModel.Field field)
        {
            var enemies = field.Enemies;
            var cubes = new List<Cube>();
            for (var i = 0; i < field.Size; i++)
            for (var j = 0; j < field.Size; j++)
            {
                switch (field[i,j])
                {
                    case GameModel.GameModel.Field.FieldElement.Void:
                        break;
                    case GameModel.GameModel.Field.FieldElement.Empty:
                        cubes.Add(new Content.Cube(field.ConvertIntoWorldCoordinates(i, j), View.Content.GreyTexture));
                        break;
                    case GameModel.GameModel.Field.FieldElement.Enemy:
                        var enemy = enemies.First(e => e.Position == new Point(i, j));
                        var cubePosition = field.ConvertIntoWorldCoordinates(i, j);
                        enemy.Cube.MoveTo(cubePosition + new Vector3(0,View.Content.CubeSizeWithOffset,0));
                        cubes.Add(enemy.Cube);
                        cubes.Add(new Content.Cube(cubePosition, View.Content.GreyTexture));
                        break;
                    case GameModel.GameModel.Field.FieldElement.Obstacle:
                        var coordinate = field.ConvertIntoWorldCoordinates(i, j);
                        cubes.Add(new Content.Cube(
                            coordinate + new Vector3(0, View.Content.CubeSizeWithOffset, 0),
                            View.Content.DarkGreyTexture));
                        cubes.Add(new Content.Cube(
                            coordinate,
                            View.Content.GreyTexture));
                        break;
                    case GameModel.GameModel.Field.FieldElement.DoubleMove:
                        cubes.Add(new Content.Cube(field.ConvertIntoWorldCoordinates(i, j), View.Content.GreenTexture));
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
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

        

        public static Architecture.Scene GetScene(int fieldSize)
        {
            var cubesPositions = new List<Vector3>();
            for (var x = -(fieldSize / 2); x <= fieldSize / 2; x++)
            {
                for (var y = -(fieldSize / 2); y <= fieldSize / 2; y++)
                {
                    cubesPositions.Add(
                        new Vector3(View.Content.CubeSizeWithOffset * x, 0, View.Content.CubeSizeWithOffset * y));
                }
            }
            var cubes = cubesPositions.Select(p => new Content.Cube(p, View.Content.RegularCube));
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

        public static Architecture.Scene GetScene() => GetScene(7);
    }
}
