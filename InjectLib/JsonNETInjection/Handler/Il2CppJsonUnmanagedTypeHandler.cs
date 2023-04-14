using Il2CppJsonNet;
using Il2CppJsonNet.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InjectLib.JsonNETInjection.Handler;
public abstract class Il2CppJsonUnmanagedTypeHandler<T> : INativeJsonHandler where T : unmanaged
{
    public abstract void OnRead(in Il2CppSystem.Object result, in JToken jToken);
}
