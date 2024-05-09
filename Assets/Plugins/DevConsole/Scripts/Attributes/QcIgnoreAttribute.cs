using System;

namespace Necroisle.DevConsole
{
    /// <summary>
    /// Instructs DevConsole to ignore this entity when scanning the code base for commands.
    /// This can be used to optimise DevConsoles loading times in large codebases when there are large entities that do not have any commands present.
    /// </summary>
    [AttributeUsage(AttributeTargets.Assembly | AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public sealed class DevConsoleIgnoreAttribute : Attribute { }
}
