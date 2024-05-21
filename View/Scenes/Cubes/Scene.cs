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

        public static Architecture.Scene GetScene(GameModel.GameModel.Field field, float distance)
        {
            var toIgnore = new List<Cube>();
            var enemies = field.Enemies;
            var cubes = new List<Cube>();
            for (var i = 0; i < field.Size; i++)
            for (var j = 0; j < field.Size; j++)
            {
                var position = field.ConvertIntoWorldCoordinates(i, j);
                switch (field[i,j])
                {
                    case GameModel.GameModel.Field.FieldElement.Void:
                        break;
                    case GameModel.GameModel.Field.FieldElement.Empty:
                        cubes.Add(new Content.Cube(position, View.Content.GreyTexture));
                        break;
                    case GameModel.GameModel.Field.FieldElement.Enemy:
                        var enemy = enemies.First(e => e.Position == new Point(i, j));
                        var underEnemy = new Content.Cube(position, View.Content.GreyTexture);

                        enemy.Cube.MoveTo(position + new Vector3(0,View.Content.CubeSizeWithOffset,0));

                        cubes.AddRange(new[] { enemy.Cube, underEnemy });
                        toIgnore.AddRange(new[] { enemy.Cube, underEnemy });
                        break;
                    case GameModel.GameModel.Field.FieldElement.Obstacle:

                        var cube = new Content.Cube(
                            position + new Vector3(0, View.Content.CubeSizeWithOffset, 0),
                            View.Content.DarkGreyTexture);

                        var underCube = new Content.Cube(
                            position,
                            View.Content.GreyTexture);

                        cubes.AddRange(new[] { cube, underCube });
                        toIgnore.AddRange(new[] { cube, underCube });
                        break;
                    case GameModel.GameModel.Field.FieldElement.DoubleMove:
                        cubes.Add(new Content.Cube(position, View.Content.GreenTexture));
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
            var result = new Scene(
                Array.Empty<Button>(),
                Array.Empty<Image>(),
                Array.Empty<Text>(),
                cubes,
                Main.Graphics,
                distance,
                Sprite.GeSolidColorTexture(Main.Graphics, Color.White,
                    GameModel.GameModel.Screen.Width, GameModel.GameModel.Screen.Height)
            );
            CreateUi(result);
            foreach (var ignoring in toIgnore)
                result.Ignore(ignoring);
            return result;
        }

        private static void CreateUi(Scene scene)
        {
            var pauseButton = new Content.PauseButton(
                new Position(new Vector2(10, 10), PositionType.Percents),
                View.Content.PauseButtonRegular,
                View.Content.PauseButtonHover,
                View.Content.PauseButtonClick,
                100, 100,
                0, new GameModel.GameModel.PauseGame());
            scene.Add(pauseButton);
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
