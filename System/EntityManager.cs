using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Confinement.Entities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Confinement.System
{
    internal class EntityManager
    {
        private readonly List<IGameEntity> _entities = new();
        private readonly List<IGameEntity> _entitiesToAdd = new();
        private readonly List<IGameEntity> _entitiesToRemove = new();

        public void Add(IGameEntity entity) =>
            _entitiesToAdd.Add(
                entity ?? throw new ArgumentNullException(nameof(entity), "Null entity cannot be added"));

        public void Remove(IGameEntity entity) =>
            _entitiesToRemove.Add(
                entity ?? throw new ArgumentNullException(nameof(entity), "Null entity cannot be removed"));

        public void Manage(GameTime gameTime, Screen screen)
        {
            
            foreach (var entity in _entities)
                entity.Update(gameTime, screen);
            foreach (var entity in _entitiesToAdd)
                _entities.Add(entity);
            foreach (var entity in _entitiesToRemove)
                _entities.Remove(entity);

            _entitiesToAdd.Clear();
            _entitiesToRemove.Clear();
        }

        public void Clear() => _entitiesToRemove.AddRange(_entities);

        public void DrawEntities(GameTime gameTime, SpriteBatch spriteBatch, Screen screen)
        {
            foreach (var entity in _entities.OrderBy(e => e.DrawOrder))
                entity.Draw(gameTime, spriteBatch, screen);
        }
    }
}
