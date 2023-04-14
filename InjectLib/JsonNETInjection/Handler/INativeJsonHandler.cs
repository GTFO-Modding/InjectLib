using Il2CppJsonNet;
using Il2CppJsonNet.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InjectLib.JsonNETInjection.Handler;
internal interface INativeJsonHandler
{
    public void OnRead(in Il2CppSystem.Object result, in JToken jToken);
}
