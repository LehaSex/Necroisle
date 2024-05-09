using System;

namespace Necroisle.DevConsole
{
    /// <summary>
    /// Exception to be thrown by an IDevConsoleParser.
    /// </summary>
    public class ParserException : Exception
    {
        public ParserException(string message) : base(message) { }
        public ParserException(string message, Exception innerException) : base(message, innerException) { }
    }
}
