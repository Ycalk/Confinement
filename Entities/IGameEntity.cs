using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Confinement.System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Confinement.Entities
{
    internal interface IGameEntity
    {
        public int DrawOrder { get; }
        public void Update(GameTime gameTime, Screen screen);
        public void Draw(GameTime gameTime, SpriteBatch spriteBatch, Screen screen);
    }
}
