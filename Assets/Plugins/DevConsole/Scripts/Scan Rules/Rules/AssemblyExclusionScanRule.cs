using System.Reflection;

namespace Necroisle.DevConsole.ScanRules
{
    public class AssemblyExclusionScanRule : IDevConsoleScanRule
    {
        public ScanRuleResult ShouldScan<T>(T entity) where T : ICustomAttributeProvider
        {
            if (entity is Assembly assembly)
            {
                string[] bannedPrefixes = new string[]
                {
                    "System", "Unity", "Microsoft", "Mono.", "mscorlib", "NSubstitute", "JetBrains", "nunit.",
                    "GeNa."
#if DevConsole_DISABLE_BUILTIN_ALL
                    , "Necroisle.DevConsole"
#elif DevConsole_DISABLE_BUILTIN_EXTRA
                    , "Necroisle.DevConsole.Extra"
#endif
                };

                string[] bannedAssemblies = new string[]
                {
                    "mcs", "AssetStoreTools",
                    "Facepunch.Steamworks"
                };

                string assemblyFullName = assembly.FullName;
                foreach (string prefix in bannedPrefixes)
                {
                    if (assemblyFullName.StartsWith(prefix))
                    {
                        return ScanRuleResult.Reject;
                    }
                }

                string assemblyShortName = assembly.GetName().Name;
                foreach (string name in bannedAssemblies)
                {
                    if (assemblyShortName == name)
                    {
                        return ScanRuleResult.Reject;
                    }
                }
            }

            return ScanRuleResult.Accept;
        }
    }
}