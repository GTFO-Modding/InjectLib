using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InjectLib.JsonNETInjection.Supports;
internal interface IBaseConnector
{
    public NativeJsonProcessor Processor { get; set; }
}
