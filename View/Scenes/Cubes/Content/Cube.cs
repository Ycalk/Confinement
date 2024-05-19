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
    internal class Cube : Architecture.Entities.Cube
    {
        public Cube(Vector3 position, Texture2D texture) 
            : base(position, View.Content.RoundedCube, texture)
        {
            SetAlfa(0.9f);
        }

        public Cube(Vector3 position, Model model, Texture2D texture)
            : base(position, model, texture)
        {
            SetAlfa(0.9f);
        }


        public Cube(Vector3 position, Model model) 
            : base(position, model)
        {
        }

        public override void OnRelease()
        {
            base.OnPress();
            GameModel.GameModel.Controller.CreateRequest(new ModelRequest(
                new GameModel.GameModel.PlayerMove(this), this));
        }
    }
}
