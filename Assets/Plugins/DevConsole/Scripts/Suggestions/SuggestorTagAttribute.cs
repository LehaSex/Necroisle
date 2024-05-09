using System;

namespace Necroisle.DevConsole
{
    /// <summary>
    /// Base attribute for all IDevConsoleSuggestorTag sources.
    /// </summary>
    [AttributeUsage(AttributeTargets.Parameter, AllowMultiple = true, Inherited = true)]
    public abstract class SuggestorTagAttribute : Attribute
    {
        public abstract IDevConsoleSuggestorTag[] GetSuggestorTags();
    }
}