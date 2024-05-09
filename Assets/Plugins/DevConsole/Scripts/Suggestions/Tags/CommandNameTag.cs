namespace Necroisle.DevConsole.Suggestors.Tags
{
    public struct CommandNameTag : IDevConsoleSuggestorTag
    {

    }

    /// <summary>
    /// Specifies that command name values should be suggested for the parameter.
    /// </summary>
    public sealed class CommandNameAttribute : SuggestorTagAttribute
    {
        private readonly IDevConsoleSuggestorTag[] _tags = { new CommandNameTag() };

        public override IDevConsoleSuggestorTag[] GetSuggestorTags()
        {
            return _tags;
        }
    }
}