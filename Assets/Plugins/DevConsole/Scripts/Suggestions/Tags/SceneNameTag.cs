namespace Necroisle.DevConsole.Suggestors.Tags
{
    public struct SceneNameTag : IDevConsoleSuggestorTag
    {
        public bool LoadedOnly;
    }

    /// <summary>
    /// Specifies that scene name values should be suggested for the parameter.
    /// </summary>
    public sealed class SceneNameAttribute : SuggestorTagAttribute
    {
        /// <summary>
        /// If true, only loaded scenes will be suggested.
        /// </summary>
        public bool LoadedOnly
        {
            get => _tag.LoadedOnly;
            set => _tag.LoadedOnly = value;
        }

        private SceneNameTag _tag;

        public override IDevConsoleSuggestorTag[] GetSuggestorTags()
        {
            return new IDevConsoleSuggestorTag[] { _tag };
        }
    }
}