using Architecture;
using Microsoft.Xna.Framework.Graphics;

namespace Confinement.View
{
    internal class Manager
    {
        private Scene _scene;

        public Manager(GameModel.GameModel.Controller controller)
        {
            controller.SceneChange += OnSceneChange;
        }

        private void OnSceneChange(Scene scene) =>
            _scene = scene;

        public void DrawScene(SpriteBatch spriteBatch) =>
            _scene.Draw(spriteBatch, GameModel.GameModel.Screen);
    }
}
