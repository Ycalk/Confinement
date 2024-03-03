using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Confinement.System
{
    internal struct Screen
    {
        public int Width { get; }
        public int Height { get; }

        public Screen(int width, int height)
        {
            Width = width;
            Height = height;
        }
    }
}
