using BepInEx;
using BepInEx.Preloader.Core.Patching;
using Mono.Cecil;
using System.Reflection;

namespace IL2CPP.Json.NET.Generator;

[PatcherPluginInfo("JsonNET.Native.Generator", "JsonNET.Native.Generator", "1.0.0")]
public class Patcher : BasePatcher
{
    public const string JsonNETFileName = "Newtonsoft.Json.dll";

    public override void Initialize()
    {
        if (Context.AvailableAssemblies.TryGetValue(JsonNETFileName, out _))
        {
            var asmPath = Context.AvailableAssembliesPaths[JsonNETFileName];
            var newAsm = AssemblyDefinition.ReadAssembly(asmPath);

            newAsm.Name.Name = "Il2CppJsonNet";
            newAsm.MainModule.Name = "Il2CppJsonNet";
            foreach (var type in newAsm.MainModule.Types)
            {
                type.Namespace = type.Namespace.Replace("Newtonsoft.Json", "Il2CppJsonNet");
            }

            var newAsmPath = Path.Combine(Paths.BepInExRootPath, "cache", "Il2CppJsonNet.dll");
            newAsm.Write(newAsmPath);

            Assembly.LoadFrom(newAsmPath);
        }
    }
}
