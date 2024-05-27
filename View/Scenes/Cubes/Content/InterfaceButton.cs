using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Architecture.Entities;
using Architecture.Entities.System;
using Confinement.GameModel;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Confinement.View.Scenes.Cubes.Content
{
    internal class InterfaceButton : Button
    {
        private readonly Sprite _hover;
        private readonly Sprite _press;
        private readonly Sprite _init;
        private readonly IGamePlot _onRelease;

        public InterfaceButton(Position position, 
            Texture2D init, Texture2D hover, Texture2D click, 
            int width, int height,
            int drawOrder, IGamePlot onRelease, string text = "") 
            : base(position, drawOrder, new Sprite(init, width, height), View.Content.Regular, Color.Black, text)
        {
            _init = new Sprite(init, width, height);
            _hover = new Sprite(hover, width, height);
            _press = new Sprite(click, width, height);
            _onRelease = onRelease;
            PressingTextColor = Color.Black;
            HoveringTextColor = Color.Black;
        }

        protected override void OnPress()
        {
            base.OnPress();
            Sprite = _press;
        }

        protected override void OnRelease()
        {
            Sprite = IsHovered ? _hover : _init;
            GameModel.GameModel.Controller.CreateRequest(new ModelRequest(
                _onRelease, this));
        }

        protected override void OnHover()
        {
            Sprite = _hover;
        }

        protected override void OnLeave()
        {
            Sprite = _init;
        }

    }
}
