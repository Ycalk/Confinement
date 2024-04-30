using Architecture;
using Microsoft.Xna.Framework.Graphics;
using static Confinement.GameModel.GameModel;

namespace Confinement.View
{
    internal class Manager
    {
        private Scene _scene;

        public Manager(Controller controller)
        {
            controller.SceneChange += OnSceneChange;
        }

        private void OnSceneChange(Scene scene) =>
            _scene = scene;

        public void DrawScene(SpriteBatch spriteBatch) =>
            _scene.Draw(spriteBatch, GameModel.GameModel.Screen);
    }
}
