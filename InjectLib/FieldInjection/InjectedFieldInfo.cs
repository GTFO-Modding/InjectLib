using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InjectLib.FieldInjection;
internal struct InjectedFieldInfo
{
    public nint FieldOffset;
    public int FieldSize;
}
