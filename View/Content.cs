using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Architecture.Entities;
using Confinement.View.Scenes.Cubes.Content;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Confinement.View
{
    internal static class Content
    {
        public const float CubeSize = 2.0f;
        public const float CubeSizeWithOffset = 2.1f;
        public static SpriteFont RegularFont { get; private set; }
        public static Model RegularCube { get; private set; }
        public static Model DoubleMoveCube { get; private set; }
        public static Model ObstacleCube { get; private set; }
        public static Model EnemyCube { get; private set; }


        public static Model RoundedCube { get; private set; }
        
        public static Texture2D GreyTexture { get; private set; }
        public static Texture2D DarkGreyTexture { get; private set; }
        public static Texture2D GreenTexture { get; private set; }
        public static Texture2D RedTexture { get; private set; }

        private static bool _contentLoaded;

        public static void LoadContent(ContentManager contentManager)
        {
            if (_contentLoaded)
                return;
            RegularFont = contentManager.Load<SpriteFont>("Fonts/Regular");

            RegularCube = contentManager.Load<Model>("Models/Regular");
            DoubleMoveCube = contentManager.Load<Model>("Models/DoubleMove");
            ObstacleCube = contentManager.Load<Model>("Models/Obstacle");
            EnemyCube = contentManager.Load<Model>("Models/Enemy");

            RoundedCube = contentManager.Load<Model>("Rounded Cube/RoundedCube");
            GreyTexture = contentManager.Load<Texture2D>("Rounded Cube/GreyTexture");
            DarkGreyTexture = contentManager.Load<Texture2D>("Rounded Cube/DarkGreyTexture");
            GreenTexture = contentManager.Load<Texture2D>("Rounded Cube/GreenTexture");
            RedTexture = contentManager.Load<Texture2D>("Rounded Cube/RedTexture");
            _contentLoaded = true;
        }

    }
}
