using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
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
        public static int ButtonsSize = 100;

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

        public static Architecture.Scene GetScene(GameModel.GameModel.Field field, float distance, int currentLevel, bool withUi = true)
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
            if (withUi)
                CreateUi(result, enemies, currentLevel);
            
            foreach (var ignoring in toIgnore)
                result.Ignore(ignoring);
            return result;
        }

        private static void CreateUi(Scene scene, ReadOnlyCollection<GameModel.GameModel.Field.Enemy> enemies, int currentLevel)
        {
            var pauseButton = new Content.InterfaceButton(
                new Position(new Vector2(10, 10), PositionType.Percents),
                View.Content.PauseButtonRegular,
                View.Content.PauseButtonHover,
                View.Content.PauseButtonClick,
                ButtonsSize, ButtonsSize,
                0, new GameModel.GameModel.PauseGame());

            var questionButton = new Content.InterfaceButton(
                new Position(new Vector2(90, 10), PositionType.Percents),
                View.Content.InstructionButtonRegular,
                View.Content.InstructionButtonHover,
                View.Content.InstructionButtonClick,
                ButtonsSize, ButtonsSize,
                0, new GameModel.GameModel.ShowInstruction());

            var text = new Text(
                new Position(50, 5, PositionType.Percents),
                0, View.Content.Regular, Color.DarkGray, "Stage     " + currentLevel);

            foreach (var button in GetEnemiesButtons(enemies))
                scene.Add(button);

            scene.Add(text);
            scene.Add(pauseButton);
            scene.Add(questionButton);
        }

        public static List<Button> GetEnemiesButtons(IReadOnlyCollection<GameModel.GameModel.Field.Enemy> enemies)
        {
            var result = new List<Button>();
            var blockLength = enemies.Count * ButtonsSize + (enemies.Count - 1) * 10;
            var startPosition =
                new Position(90, 50, PositionType.Percents).GetCoordinate(GameModel.GameModel.Screen, ButtonsSize,
                    blockLength);
            var currentY = startPosition.Y;

            foreach (var enemy in enemies)
            {
                var button = new Content.InterfaceButton(
                       new Position(new Vector2(startPosition.X, currentY), PositionType.Pixels),
                      View.Content.CubeButtonRegular,
                      View.Content.CubeButtonHover,
                      View.Content.CubeButtonClick,
                      ButtonsSize, ButtonsSize,
                      0, new GameModel.GameModel.ChangeCameraTarget(enemy));
                currentY += ButtonsSize + 10;
                result.Add(button);
            }

            return result;
        }

    }
}
