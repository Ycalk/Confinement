using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Architecture;
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
        public readonly IGamePlot ReleaseAction;

        private Sprite _spriteBeforeIgnoring;
        
        public bool OnPause { get; private set; }

        public InterfaceButton(Position position, 
            Texture2D init, Texture2D hover, Texture2D click, 
            int width, int height,
            int drawOrder, IGamePlot releaseAction, string text = "") 
            : base(position, drawOrder, new Sprite(init, width, height), View.Content.Regular, Color.Black, text)
        {
            _init = new Sprite(init, width, height);
            _hover = new Sprite(hover, width, height);
            _press = new Sprite(click, width, height);
            ReleaseAction = releaseAction;
            PressingTextColor = Color.White;
            HoveringTextColor = Color.Black;
        }

        protected override void OnPress()
        {
            base.OnPress();
            Sprite = _press;
        }

        protected override void OnRelease()
        {
            base.OnRelease();
            Sprite = IsHovered ? _hover : _init;
            GameModel.GameModel.Controller.CreateRequest(new ModelRequest(
                ReleaseAction, this));
        }

        protected override void OnHover()
        {
            base.OnHover();
            Sprite = _hover;
        }

        protected override void OnLeave()
        {
            base.OnLeave();
            Sprite = _init;
        }

        public void Pause()
        {
            OnPause = true;
            _spriteBeforeIgnoring = Sprite;
            Sprite = new Sprite(Sprite.CombineTextures(Main.Graphics, Sprite.Texture, View.Content.RoundedTranslucentBlack),
                Width, Height);
        }

        public void UnPause()
        {
            OnPause = false;
            Sprite = _spriteBeforeIgnoring;
        }

    }
}
