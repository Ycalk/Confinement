using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Architecture;
using Microsoft.Xna.Framework.Graphics;

namespace Confinement.View.System
{
    internal interface ISceneConstructor
    {
        public static Scene GetScene()
        {
            throw new NotImplementedException();
        }
    }
}
