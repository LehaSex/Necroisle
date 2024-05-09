namespace Necroisle.DevConsole.Parsers
{
    public class StringParser : BasicCachedDevConsoleParser<string>
    {
        public override int Priority => int.MaxValue;

        public override string Parse(string value)
        {
            return value
                .ReduceScope('"', '"')
                .UnescapeText('"');
        }
    }
}
