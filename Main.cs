using System.Diagnostics;
using Confinement.System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Confinement
{
    public class Main : Game
    {
        

        public const string GameName = "Confinement";

        private readonly GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private readonly EntityManager _entityManager;

        public Main()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
            _entityManager = new EntityManager();
        }

        protected override void Initialize()
        {
            base.Initialize();

            if (GraphicsDevice is null)
                _graphics.ApplyChanges();

            
            Window.AllowUserResizing = true;
            Window.Title = GameName;
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();
            _entityManager.Manage(gameTime, new Screen(Window.ClientBounds.Width, Window.ClientBounds.Height));
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.White);
            _spriteBatch.Begin();
            _entityManager.DrawEntities(gameTime, _spriteBatch, new Screen(Window.ClientBounds.Width, Window.ClientBounds.Height));
            _spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
