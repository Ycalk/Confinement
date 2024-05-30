using static Confinement.GameModel.GameModel;
using System.Diagnostics;
using System;
using Architecture.Entities;
using Architecture.Entities.System;
using Confinement.View;
using System.Collections.Generic;
using Confinement.View.Scenes.Cubes.Content;
using Microsoft.Xna.Framework;

namespace Confinement.GameModel
{
    internal partial class GameModel
    {
        internal class ShowInstruction : IGamePlot
        {
            public const string InstructionText =
                "Confinement is a turn-based game|where you have to stop the Red cubes|from leaving the field.|" +
                "At every step you place obstacles|to prevent the cubes from|getting out of the field.| |" +
                "On the field there are green cells;|stepping on them will cause the Red cube|to make a double move.| |" +
                "Use arrows to scale or rotate the field.| |Good luck!";


            public void Execute(Entity sender)
            {
                if (new StackTrace().GetFrame(1)!.GetMethod()!.DeclaringType != typeof(Controller))
                    throw new InvalidOperationException("Method can only be execute in controller");

                var textFont = Content.Regular;

                var textCurrentHeight = 0f;
                var instructionLines = InstructionText.Split('|');
                var texts = new List<Text>();
                foreach (var line in instructionLines)
                {
                    var text = line.Replace(" ", "     ");
                    var (width, height) = textFont.MeasureString(text);
                    var textPosition = new Position(50, 50, PositionType.Percents)
                        .GetCoordinate(Screen, (int)width, (int)height);
                    texts.Add(new Text(
                        new Position(textPosition.X, textPosition.Y + textCurrentHeight - (instructionLines.Length * height) / 2, 
                            PositionType.Pixels),
                        1, textFont, Color.Black, text));
                    textCurrentHeight += height;
                }
                

                var entities = new List<Entity>();

                entities.AddRange(PauseGame.GetPauseForm());
                entities.AddRange(texts);

                _controller.PauseGame(entities);
            }
        }
    }
}
