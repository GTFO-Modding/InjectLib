using Il2CppInterop.Runtime.InteropTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InjectLib.FieldInjection;

// ❤️ This whole idea has Stolen from Kas ❤️
public static partial class FieldInjector<Il2CppType> where Il2CppType : Il2CppObjectBase
{
    private static readonly Dictionary<string, InjectedFieldInfo> _OffsetLookup = new();

    public static unsafe void DefineValueField<Value>(string fieldName) where Value : unmanaged
    {
        if (_OffsetLookup.ContainsKey(fieldName))
        {
            return;
        }

        InjectValueField<Value>(fieldName);
    }

    public static unsafe void DefineManagedField<Value>(string fieldName) where Value : class
    {
        if (_OffsetLookup.ContainsKey(fieldName))
        {
            return;
        }

        InjectManagedField<Value>(fieldName);
    }
}
