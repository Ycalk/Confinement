using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Confinement.GameModel.GameModes
{
    internal interface IGameMode
    {
        public int CurrentLevel { get; set; }
        public void OnPlayerWin();
        public void OnPlayerLose();
    }
}
