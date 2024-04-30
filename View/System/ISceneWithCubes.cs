using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Architecture.Entities;

namespace Confinement.View.System
{
    internal interface ISceneWithCubes
    {
        public void AddCube(Cube cube);
        public void RemoveCube(Cube cube);
        public void IgnoreCube(Cube cube);
    }
}
