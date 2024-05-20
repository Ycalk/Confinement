using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Architecture.Entities;
using Confinement.View.Scenes.Cubes.Content;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Confinement.View
{
    internal static class Content
    {
        public const float CubeSize = 2.0f;
        public const float CubeSizeWithOffset = 2.1f;
        public static SpriteFont Regular { get; private set; }
        public static SpriteFont GameName { get; private set; }

        public static Texture2D ButtonRegular { get; private set; }
        public static Texture2D ButtonHover { get; private set; }
        public static Texture2D ButtonClick { get; private set; }

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
            Regular = contentManager.Load<SpriteFont>("Fonts/Regular");
            GameName = contentManager.Load<SpriteFont>("Fonts/GameName");

            RegularCube = contentManager.Load<Model>("Models/Regular");
            DoubleMoveCube = contentManager.Load<Model>("Models/DoubleMove");
            ObstacleCube = contentManager.Load<Model>("Models/Obstacle");
            EnemyCube = contentManager.Load<Model>("Models/Enemy");

            RoundedCube = contentManager.Load<Model>("RoundedCube/RoundedCube");
            GreyTexture = contentManager.Load<Texture2D>("RoundedCube/GreyTexture");
            DarkGreyTexture = contentManager.Load<Texture2D>("RoundedCube/DarkGreyTexture");
            GreenTexture = contentManager.Load<Texture2D>("RoundedCube/GreenTexture");
            RedTexture = contentManager.Load<Texture2D>("RoundedCube/RedTexture");

            ButtonRegular = contentManager.Load<Texture2D>("Buttons/ButtonRegular");
            ButtonHover = contentManager.Load<Texture2D>("Buttons/ButtonHover");
            ButtonClick = contentManager.Load<Texture2D>("Buttons/ButtonClick");
            _contentLoaded = true;
        }

    }
}
