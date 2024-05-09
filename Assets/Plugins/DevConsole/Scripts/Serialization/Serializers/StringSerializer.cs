namespace Necroisle.DevConsole.Serializers
{
    public class StringSerializer : BasicDevConsoleSerializer<string>
    {
        public override int Priority => int.MaxValue;

        public override string SerializeFormatted(string value, QuantumTheme theme)
        {
            return value;
        }
    }
}
