using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Architecture.Entities.System;

namespace Confinement.GameModel
{
    internal class ModelRequest
    {
        public IGamePlot Request { get; }
        public Entity Sender { get; }

        public ModelRequest(IGamePlot request, Entity sender)
        {
            Request = request;
            Sender = sender;
        }
    }
}
