using Architecture.Entities.System;
using Confinement.GameModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Architecture.Entities;
using Microsoft.Xna.Framework;

namespace Confinement.View.Scenes.MainMenu.Content
{
    internal class MainMenuButton : Button
    {
        private Sprite Hover => new(View.Content.ButtonHover, Width, Height);
        private Sprite Press => new(View.Content.ButtonClick, Width, Height);
        private readonly Sprite _initSprite;
        public readonly Position InitPosition;
        private readonly int _offset = 20;
        private readonly IGamePlot _onRelease;

        public MainMenuButton(Position position, int drawOrder, Sprite sprite, IGamePlot onRelease, string text)
            : base(position, drawOrder, sprite, View.Content.Regular, Color.Black, text)
        {
            _initSprite = sprite;
            InitPosition = position;
            HoveringTextColor = Color.Black;
            PressingTextColor = Color.White;
            _onRelease = onRelease;
        }

        protected override void OnPress()
        {
            base.OnPress();
            Sprite = Press;
        }

        protected override void OnRelease()
        {
            Sprite = IsHovered ? Hover : _initSprite;
            GameModel.GameModel.Controller.CreateRequest(new ModelRequest(
               _onRelease, this));
        }

        protected override void OnHover()
        {
            base.OnRelease();
            Width += _offset;
            Height += _offset;
            Position = new Position(Position.LastCoordinates - new Vector2(_offset / 2, _offset / 2), PositionType.Pixels);
            Sprite = Hover;
        }

        protected override void OnLeave()
        {
            base.OnRelease();
            Width -= _offset;
            Height -= _offset;
            Position = InitPosition;
            Sprite = _initSprite;
        }
    }
}
