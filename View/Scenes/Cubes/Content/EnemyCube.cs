using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Confinement.View.Scenes.Cubes.Content
{
    internal class EnemyCube : Cube
    {
        public EnemyCube() : base(Vector3.Zero, View.Content.EnemyCube)
        {
        }
    }
}
