using Il2CppJsonNet.Converters;
using InjectLib.FieldInjection;
using InjectLib.JsonNETInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InjectLib.Utils;
internal static class FieldsExtension
{
    public static void SetNativeJsonProcessor(this DataSetConverter converter, NativeJsonProcessor processor)
    {
        FieldInjector<DataSetConverter>.TrySetManagedField(converter, "m_JsonProcessor", processor);
    }

    public static NativeJsonProcessor GetNativeJsonProcessor(this DataSetConverter converter)
    {
        FieldInjector<DataSetConverter>.TryGetManagedField<NativeJsonProcessor>(converter, "m_JsonProcessor", out var processor);
        return processor;
    }
}
