using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonoGameWindowsStarter
{
    public interface IBoundable
    {
        BoundingRectangle Bounds { get; }
    }
}
