using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Architecture.Entities;
using Architecture.Entities.System;
using Confinement.GameModel.GameModes;
using Confinement.View;
using Confinement.View.Scenes.MainMenu.Content;
using Microsoft.Xna.Framework;

namespace Confinement.GameModel
{
    internal partial class GameModel
    {
        internal class EnemyCountMenu : IGamePlot
        {
            private const int PauseFormSize = 800;

            public void Execute(Entity sender)
            {
                if (new StackTrace().GetFrame(1)!.GetMethod()!.DeclaringType != typeof(Controller))
                    throw new InvalidOperationException("Method can only be execute in controller");

                _currentScene.Remove(sender);

                var entities = new List<Entity>();
                if (sender is not MainMenuButton button)
                    throw new InvalidOperationException("Invalid sender");


                var senderPosition = button.InitPosition.GetCoordinate(Screen, View.Scenes.MainMenu.Scene.ButtonsWidth,
                    View.Scenes.MainMenu.Scene.ButtonsHeight);

                entities.AddRange(GetEnemyCountButton(senderPosition));
                foreach (var entity in entities)
                    _currentScene.Add(entity);
            }
            private List<Button> GetEnemyCountButton(Vector2 senderPosition)
            {
                var result = new List<Button>();
                

                var blockLength = Field.MaximalEnemyCount * 100 + (Field.MaximalEnemyCount - 1) * 10;
                
                var startPosition =
                    new Position(10, 50, PositionType.Percents).GetCoordinate(Screen, blockLength,
                        100);
                var positionY = senderPosition.Y;

                var currentX = startPosition.X;

                for (var i = 1; i < Field.MaximalEnemyCount + 1; i++)
                {
                    var button = new View.Scenes.Cubes.Content.InterfaceButton(
                           new Position(currentX, positionY, PositionType.Pixels),
                          View.Content.EmptyButtonRegular,
                          View.Content.EmptyButtonHover,
                          View.Content.EmptyButtonClick,
                          100, 100, 0, new StartGame(i, new LevelSelect(i)),
                          i.ToString());
                    result.Add(button);
                    currentX += 100 + 10;
                }

                return result;
            }
        }
    }
}
