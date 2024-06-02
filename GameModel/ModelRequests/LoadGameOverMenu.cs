using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Architecture.Entities;
using Architecture.Entities.System;
using Confinement.View;

namespace Confinement.GameModel
{
    internal partial class GameModel
    {
        internal class LoadGameOverMenu : IGamePlot
        {
            private readonly int _menuWidth;
            private readonly int _menuHeight;
            private readonly IEnumerable<Button> _buttons;
            private readonly Text _text;
            public void Execute(Entity sender)
            {
                if (new StackTrace().GetFrame(1)!.GetMethod()!.DeclaringType != typeof(Controller))
                    throw new System.InvalidOperationException("Method can only be execute in controller");

                var buttons = _currentScene.GetEntities<Button>().ToArray();

                foreach (var button in buttons)
                    _currentScene.Remove(button);

                var texts = _currentScene.GetEntities<Text>().ToArray();
                foreach (var text in texts)
                    _currentScene.Remove(text);

                var cubes = _currentScene.GetEntities<Cube>().ToArray();
                foreach (var cube in cubes)
                    _currentScene.Ignore(cube);

                var menuBack = new Image(new Position(50, 50, PositionType.Percents),
                    new Sprite(Content.TranslucentBlack, Screen.Width, Screen.Height), 9);

                var menu = new Image(new Position(50, 50, PositionType.Percents),
                    new Sprite(Content.PauseTexture, _menuWidth, _menuHeight), 10);

                _currentScene.Add(menu);
                _currentScene.Add(menuBack);
                _currentScene.Add(_text);
                foreach (var button in _buttons)
                    _currentScene.Add(button);

            }

            public LoadGameOverMenu(int menuWidth, int menuHeight, IEnumerable<Button> buttons, Text text)
            {
                _menuWidth = menuWidth;
                _menuHeight = menuHeight;
                _buttons = buttons;
                _text = text;
            }
        }
    }
}
