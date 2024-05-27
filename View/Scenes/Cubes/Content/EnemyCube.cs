using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Confinement.GameModel;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Confinement.View.Scenes.Cubes.Content
{
    internal class EnemyCube : Cube
    {
        public EnemyCube() : base(Vector3.Zero, View.Content.RedTexture)
        {
        }

        public override void OnRelease()
        {
            GameModel.GameModel.Controller.CreateRequest(new ModelRequest(
                new GameModel.GameModel.ChangeCameraTarget(Position), this));
        }
    }
}
