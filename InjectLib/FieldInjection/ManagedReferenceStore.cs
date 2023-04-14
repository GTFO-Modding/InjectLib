using Il2CppInterop.Runtime.InteropTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InjectLib.FieldInjection;
public static class ManagedReferenceStore<C, T> where C : Il2CppObjectBase where T : class
{
    private static readonly Dictionary<ulong, T> _Lookup = new();
    private static ulong _Key = 1ul;

    public static ulong UniqueKey { get => _Key++; }

    public static T Get(ulong key)
    {
        _Lookup.TryGetValue(key, out var result);
        return result;
    }

    public static void Set(ulong key, T value)
    {
        _Lookup[key] = value;
    }

    public static void Remove(ulong key)
    {
        _Lookup.Remove(key);
    }
}
