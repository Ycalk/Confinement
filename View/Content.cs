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
            _contentLoaded = true;
        }

    }
}
