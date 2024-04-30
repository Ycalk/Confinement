using Architecture.Entities;
using Architecture.Entities.System;
using Confinement.GameModel;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Confinement.View.Scenes.MainMenu.Content
{
    internal class StartButton : Button
    {
        private readonly GraphicsDevice _graphics;
        private readonly Sprite _initSprite;
        private Sprite Hover => new(_graphics, Color.Aqua, Width, Height);
        private Sprite Press => new(_graphics, Color.Red, Width, Height);
        private readonly Position _initPosition;
        private readonly int _offset = 20;
        public StartButton(GraphicsDevice graphics, Position position, int drawOrder, Sprite sprite)
            : base(position, drawOrder, sprite, View.Content.RegularFont, Color.White, "Start")
        {
            _graphics = graphics;
            _initSprite = sprite;
            _initPosition = position;
            HoveringTextColor = Color.Black;
        }

        protected override void OnPress()
        {
            Sprite = Press;
            GameModel.GameModel.Controller.CreateRequest(
                new ModelRequest(c => c.LoadScene(Cubes.Scene.GetScene()), this));
        }

        protected override void OnRelease()
        {
            Sprite = IsHovered ? Hover : _initSprite;
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
            Position = _initPosition;
            Sprite = _initSprite;
        }
    }
}