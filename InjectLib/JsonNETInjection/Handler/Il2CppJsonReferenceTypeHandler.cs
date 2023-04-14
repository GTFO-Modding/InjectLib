using Il2CppInterop.Runtime.InteropTypes;
using Il2CppJsonNet;
using Il2CppJsonNet.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InjectLib.JsonNETInjection.Handler;
public abstract class Il2CppJsonReferenceTypeHandler<T> : INativeJsonHandler where T : Il2CppObjectBase
{
    public abstract void OnRead(in Il2CppSystem.Object result, in JToken jToken);
}
