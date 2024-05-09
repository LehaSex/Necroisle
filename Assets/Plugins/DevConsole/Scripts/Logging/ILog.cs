using UnityEngine;

namespace Necroisle.DevConsole
{
    public interface ILog
    {
        string Text { get; }
        LogType Type { get; }
        bool NewLine { get; }
    }
}