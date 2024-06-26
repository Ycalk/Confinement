﻿

using Architecture;
using Confinement.View;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Confinement
{
    public class Main : Game
    {
        public const string GameName = "Confinement";
        public static GraphicsDevice Graphics { get; private set; }

        private readonly GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private readonly GameModel.GameModel.Controller _controller;
        private readonly Player _player;
        private readonly Manager _viewManager;

        public Main()
        {
            _graphics = new GraphicsDeviceManager(this);
            
            _graphics.PreferMultiSampling = true;
            Content.RootDirectory = "Content";
            _graphics.IsFullScreen = true;
            _graphics.PreferredBackBufferWidth = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width;
            _graphics.PreferredBackBufferHeight = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height;
            _player = new Player();
            var mouseState = Mouse.GetState();
            _graphics.ApplyChanges();
            _controller = new GameModel.GameModel.Controller(_player,
                new Screen(_graphics.GraphicsDevice.Viewport.Width, _graphics.GraphicsDevice.Viewport.Height, mouseState.X, mouseState.Y), Exit);
            _viewManager = new Manager(_controller);
        }

        protected override void Initialize()
        {
            base.Initialize();

            if (GraphicsDevice is null)
                _graphics.ApplyChanges();
            IsMouseVisible = true;
            Window.Title = GameName;
        }

        protected override void LoadContent()
        {
            Graphics = _graphics.GraphicsDevice;
            _graphics.GraphicsDevice.PresentationParameters.MultiSampleCount = 10;
            _graphics.ApplyChanges();
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            View.Content.LoadContent(Content);
            _controller.StartModel();
        }

        protected override void Update(GameTime gameTime)
        {
            _player.ProcessControls(Window.ClientBounds.Width, Window.ClientBounds.Height);
            _controller.Manage(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.White);
            _spriteBatch.Begin();
            _viewManager.DrawScene(_spriteBatch);
            _spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
